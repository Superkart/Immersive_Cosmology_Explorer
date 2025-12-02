using UnityEngine;
using UnityEngine.UI;

public class UiManager_Vr : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject resumeButton;
    public GameObject exitButton;

    public DataManipulator dataManipulator;
    public NewDataImporter dataImporter;
    void Start()
    {
        resumeButton.SetActive(false);
 
    }


    public void ResumeVisualization()
    {
        dataImporter.Resume();
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }

    public void TogglePauseTime()
    {
        dataImporter.Pause();
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);

    }

    public void ExitVisualization()
    {
        Application.Quit();
    }
}
