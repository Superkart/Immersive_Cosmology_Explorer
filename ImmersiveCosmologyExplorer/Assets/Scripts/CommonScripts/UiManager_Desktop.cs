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

    private string sessionsFolder;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Folder where all sessions are saved
            sessionsFolder = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                "ICE_Sessions"
            );

            if (!Directory.Exists(sessionsFolder))
                Directory.CreateDirectory(sessionsFolder);

            RefreshSessionDropdown();
            InitializeUI();
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
            Debug.LogError("❌ Selected folder does not contain valid ICE dataset files!");
            datasetPathText.text = "Invalid dataset folder!";
            return;
        }

        // If valid → update UI
        datasetPathText.text = folder;

        // store in session manager
        SessionManager.Instance.selectedDataFolder = folder;

        // compute datasetID
        SessionManager.Instance.currentDatasetID = SimpleHash(folder);

        Debug.Log("Dataset validated and loaded. ID: " + SessionManager.Instance.currentDatasetID);
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
            Debug.LogError("❌ No dataset selected!");
            return;
        }

        // Validate dataset folder
        if (!FolderContainsValidDataset(SessionManager.Instance.selectedDataFolder))
        {
            Debug.LogError("❌ Selected folder does not contain a valid ICE dataset!");
            return;
        }

        // ------------------------------
        // FIX: ONLY validate session if user actually selected a saved session
        // ------------------------------
        bool userSelectedSession =
            !string.IsNullOrEmpty(SessionManager.Instance.selectedSessionName);

        if (userSelectedSession)
        {
            var ses = SessionManager.Instance.loadedSessionData;

            if (ses == null)
            {
                Debug.LogError("❌ Session selection is invalid!");
                return;
            }

            if (ses.datasetId != SessionManager.Instance.currentDatasetID)
            {
                Debug.LogError("❌ Selected session does NOT belong to this dataset!");
                return;
            }

            Debug.Log("Session validated successfully.");
        }
        else
        {
            Debug.Log("➡ No saved session selected. Starting fresh.");
        }

        // Start Visualization Scene
        FindObjectOfType<AppFlowManager>().StartVisualizationScene();
    }



    // ---------------------------------------------------------
    // REFRESH SESSION DROPDOWN
    // ---------------------------------------------------------
    private void RefreshSessionDropdown()
    {
        sessionDropdown.ClearOptions();

        List<string> options = new List<string>();
        options.Add("New Session");

        if (Directory.Exists(sessionsFolder))
        {
            foreach (string file in Directory.GetFiles(sessionsFolder, "*.json"))
            {
                options.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        sessionDropdown.AddOptions(options);
    }

    // ---------------------------------------------------------
    private string SimpleHash(string text)
    {
        int hash = text.GetHashCode();
        return hash.ToString("X");
    }
}
