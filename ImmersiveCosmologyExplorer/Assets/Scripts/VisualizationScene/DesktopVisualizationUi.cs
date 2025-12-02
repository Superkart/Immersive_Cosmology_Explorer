using UnityEngine;
using UnityEngine.UI;

public class DesktopVisualizationUI : MonoBehaviour
{
    public DataManipulator manipulator;
    public NewDataImporter importer;
    public SessionSaver saver;

    [Header("Sliders")]
    public Slider transparencySlider;
    public Slider sizeSlider;

    public Slider sizeSlider_VR;
    public Slider transparencySlider_VR;

    [Header("Buttons")]
    public Button pauseButton;
    public Button resumeButton;
    public GameObject pauseGameObject;
    public GameObject resumeGameObject;



    public Button saveSessionButton;


    public GameObject hideControlsButton;
    public GameObject showControlsButton;
    public GameObject controlPanel;


    public GameObject exitMenu;
    public GameObject exitMenu_VR;
    void Start()
    {
        exitMenu_VR.SetActive(false);
        exitMenu.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        controlPanel.SetActive(true);
        showControlsButton.SetActive(false);
        // Correct callbacks for NEW shader
        transparencySlider.onValueChanged.AddListener(OnTransparencyChange);
        sizeSlider.onValueChanged.AddListener(OnSizeChange);

        sizeSlider_VR.onValueChanged.AddListener(OnSizeChange);
        transparencySlider_VR.onValueChanged.AddListener(OnTransparencyChange);

        pauseButton.onClick.AddListener(PauseTime);
        resumeButton.onClick.AddListener(ResumeTime);

        saveSessionButton.onClick.AddListener(SaveSession);
    }


    void Update()
    {

        sizeSlider.SetValueWithoutNotify(manipulator.currentPointSize);
        transparencySlider.SetValueWithoutNotify(manipulator.currentAlpha);
        sizeSlider_VR.SetValueWithoutNotify(manipulator.currentPointSize);
        transparencySlider_VR.SetValueWithoutNotify(manipulator.currentAlpha);
    }


    public void OnTransparencyChange(float value)
    {
        manipulator.SetAlpha(value);  // updates _GlobalAlpha
    }

    public void OnSizeChange(float value)
    {
        manipulator.SetPointSize(value); // updates _PointSize
    }

    public void PauseTime()
    {
        importer.Pause();
        resumeButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void ResumeTime()
    {
        importer.Resume();
        resumeButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void SaveSession()
    {
        saver.SaveSession();
    }

    public void HideControls()
    {
        controlPanel.SetActive(false);
        hideControlsButton.SetActive(false);
        showControlsButton.SetActive(true);
    }
    public void ShowControls()
    {
        controlPanel.SetActive(true);
        hideControlsButton.SetActive(true);
        showControlsButton.SetActive(false);
    }

    public void OpenExitMenu()
    {
        Time.timeScale = 0f;
        exitMenu.SetActive(true);
    }
    public void Exitgame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        exitMenu.SetActive(false);
    }

    public void SaveAndExit()
    {
        saver.SaveSession();
        Application.Quit();
    }

    public void OpenExitMenu_VR()
    {
        Time.timeScale = 0f;
        exitMenu_VR.SetActive(true);
    }

    public void ResumeGame_VR()
    {
        Time.timeScale = 1f;
        exitMenu_VR.SetActive(false);
    }
}
