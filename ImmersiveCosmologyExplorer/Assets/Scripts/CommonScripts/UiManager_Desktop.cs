using UnityEngine;
using SFB;

public class UiManager_Desktop : MonoBehaviour
{
    [SerializeField] private GameObject LoadDataPanel;
    [SerializeField] private GameObject LoadSessionPanel;

    public NewDataImporter dataImporter; // assign in inspector

    void Start()
    {
        ShowLoadDataPanel();
    }

    public void ShowLoadDataPanel()
    {
        LoadDataPanel.SetActive(true);
        LoadSessionPanel.SetActive(false);
    }

    public void ShowSessionPanel()
    {
        LoadDataPanel.SetActive(false);
        LoadSessionPanel.SetActive(true);
    }

    // Called by Load Button
    public void OnLoadDataClicked()
    {
        string[] folders = StandaloneFileBrowser.OpenFolderPanel("Select Data Folder", "", false);

        if (folders.Length > 0)
        {
            string folderPath = folders[0];
            Debug.Log("Selected folder: " + folderPath);

            dataImporter.LoadDataFromFolder(folderPath);

            ShowSessionPanel(); // Move to next panel
        }
    }

}
