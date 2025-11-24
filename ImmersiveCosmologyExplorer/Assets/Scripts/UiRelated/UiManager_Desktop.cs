using UnityEngine;
using SFB;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UiManager_Desktop : MonoBehaviour
{
    [SerializeField] private List<GameObject> ListOfMenus;
    public NewDataImporter dataImporter; // assign in inspector

    void Start()
    {
        DisableMenus();
        ShowPanel(ListOfMenus[0]);
    }

    public void ShowPanel(GameObject Menu)
    {
        DisableMenus();
        Menu.SetActive(true);
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
        }

        ShowPanel(ListOfMenus[3]);
        DisableMenu(ListOfMenus[0]);
    }

    private void DisableMenus()
    {
        for(int i = 0; i < ListOfMenus.Count; i++)
        {
            ListOfMenus[i].SetActive(false);
        }
    }
    private void DisableMenu(GameObject Menu)
    {
        Menu.SetActive(false);
    }

}
