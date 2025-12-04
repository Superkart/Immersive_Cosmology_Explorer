using UnityEngine;
using TMPro;
using SFB;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class DesktopUiManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text datasetPathText;
    public TMP_Dropdown sessionDropdown;
    public GameObject warningDatasetLoad;
    public GameObject visualizeButton;

    public FlashObject flashObject;
    private string sessionsFolder;

    private void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            visualizeButton.SetActive(false);
            warningDatasetLoad.SetActive(false);
            Debug.Log("⚡ DesktopUiManager.Start() running...");

            sessionsFolder = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                "ICE_Sessions"
            );

            Debug.Log("Using Sessions Folder: " + sessionsFolder);

            if (!Directory.Exists(sessionsFolder))
                Directory.CreateDirectory(sessionsFolder);

            InitializeUI();

            RefreshSessionDropdown();   // MUST BE HERE
        }
    }




    private void InitializeUI()
    {
        datasetPathText.text = "No dataset selected";
        sessionDropdown.value = 0;
    }

    // ---------------------------------------------------------
    // LOAD DATASET BUTTON CLICKED
    // ---------------------------------------------------------


    private bool FolderContainsValidDataset(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            return false;

        // must contain at least one CSV matching the expected pattern
        string[] files = Directory.GetFiles(folderPath, "m000p.particles-*.csv");

        return files.Length > 0;
    }

    public void OnClick_LoadDataFolder()
    {
        string[] paths = StandaloneFileBrowser.OpenFolderPanel("Select Data Folder", "", false);

        if (paths.Length == 0)
            return;

        string folder = paths[0];

        // VALIDATION
        if (!FolderContainsValidDataset(folder))
        {
            datasetPathText.text = "Invalid dataset folder!";
            flashObject.Flash();
            visualizeButton.SetActive(false);
            return;
        }
        string folderName = Path.GetFileName(folder);
        // If valid → update UI
        // datasetPathText.text = folder;
        datasetPathText.text = folderName;

        // store in session manager
        SessionManager.Instance.selectedDataFolder = folder;

        // compute datasetID
        SessionManager.Instance.currentDatasetID = SimpleHash(folder);

        Debug.Log("Dataset validated and loaded. ID: " + SessionManager.Instance.currentDatasetID);
        RefreshSessionDropdown();
        visualizeButton.SetActive(true);

    }

    // ---------------------------------------------------------
    // SESSION SELECTED FROM DROPDOWN
    // ---------------------------------------------------------
    public void OnSelect_Session(int index)
    {
        string chosen = sessionDropdown.options[index].text;




        if (chosen == "New Session")
        {
            SessionManager.Instance.loadedSessionData = null;
            SessionManager.Instance.selectedSessionName = "";
            return;
        }

        // ERROR: session selected but no dataset loaded
        if (string.IsNullOrEmpty(SessionManager.Instance.selectedDataFolder))
        {
            flashObject.Flash();   // shows UI for 2 seconds
            return;
        }


        // Load JSON
        string filePath = Path.Combine(sessionsFolder, chosen + ".json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("Session file not found: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        SessionData session = JsonUtility.FromJson<SessionData>(json);

        SessionManager.Instance.loadedSessionData = session;
        SessionManager.Instance.selectedSessionName = chosen;

        Debug.Log("Session loaded: " + chosen);
    }


    // ---------------------------------------------------------
    // START VISUALIZATION BUTTON
    // ---------------------------------------------------------
    public void OnClick_StartVisualization()
    {
        // Require dataset
        if (string.IsNullOrEmpty(SessionManager.Instance.selectedDataFolder))
        {
            flashObject.Flash();

            return;
        }

        // Validate dataset folder
        if (!FolderContainsValidDataset(SessionManager.Instance.selectedDataFolder))
        {
            Debug.LogError("❌ Selected folder does not contain a valid ICE dataset!");
            return;
        }

        // --------------------------------------
        // ⭐ OVERRIDE: Load session directly here
        // --------------------------------------
        string dropdownText = sessionDropdown.options[sessionDropdown.value].text;

        Debug.Log("⭐ StartVisualization() selected session: " + dropdownText);

        if (dropdownText == "New Session")
        {
            SessionManager.Instance.loadedSessionData = null;
            SessionManager.Instance.selectedSessionName = "";
        }
        else
        {
            string filePath = Path.Combine(sessionsFolder, dropdownText + ".json");
            if (!File.Exists(filePath))
            {
                Debug.LogError("❌ Session file missing: " + filePath);
            }
            else
            {
                string json = File.ReadAllText(filePath);
                SessionData session = JsonUtility.FromJson<SessionData>(json);

                SessionManager.Instance.loadedSessionData = session;
                SessionManager.Instance.selectedSessionName = dropdownText;

                Debug.Log("⭐ Loaded session successfully in StartVisualization()");
            }
        }

        // --------------------------------------
        // NOW start the visualization scene
        // --------------------------------------
        FindObjectOfType<AppFlowManager>().StartVisualizationScene();
    }


    // ---------------------------------------------------------
    // REFRESH SESSION DROPDOWN
    // ---------------------------------------------------------
    private void RefreshSessionDropdown()
    {
        Debug.Log("⚡ RefreshSessionDropdown CALLED");

        sessionDropdown.ClearOptions();

        List<string> options = new List<string>();
        options.Add("New Session");

        string currentDatasetID = SessionManager.Instance.currentDatasetID;

        if (Directory.Exists(sessionsFolder) && !string.IsNullOrEmpty(currentDatasetID))
        {
            foreach (string file in Directory.GetFiles(sessionsFolder, "*.json"))
            {
                string json = File.ReadAllText(file);
                SessionData sd = JsonUtility.FromJson<SessionData>(json);

                if (sd.datasetId == currentDatasetID)
                {
                    options.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
        }

        sessionDropdown.AddOptions(options);

        // ⭐ FIX: Defer selection to next frame so TMP can rebuild
        StartCoroutine(SelectDefaultSessionNextFrame());
    }

    private System.Collections.IEnumerator SelectDefaultSessionNextFrame()
    {
        yield return null; // wait 1 frame

        sessionDropdown.value = 0;   // select first option or preserve previous selection
        sessionDropdown.RefreshShownValue();

        // If more than 1 option exists, auto-select saved session
        // Otherwise it will stay on "New Session"
        OnSelect_Session(sessionDropdown.value);

        Debug.Log("✔ Session selected after TMP refresh: " + sessionDropdown.options[sessionDropdown.value].text);
    }



    // ---------------------------------------------------------
    private string SimpleHash(string text)
    {
        int hash = text.GetHashCode();
        return hash.ToString("X");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

}
