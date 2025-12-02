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

    public Button saveSessionButton;


    public GameObject hideControlsButton;
    public GameObject showControlsButton;
    public GameObject controlPanel;

    void Start()
    {

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
    }

    public void ResumeTime()
    {
        importer.Resume();
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
}
