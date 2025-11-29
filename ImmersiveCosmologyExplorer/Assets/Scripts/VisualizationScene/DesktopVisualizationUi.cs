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

    [Header("Buttons")]
    public Button pauseButton;
    public Button resumeButton;
    public Button saveSessionButton;

    void Start()
    {
        // Wire slider callbacks
        transparencySlider.onValueChanged.AddListener(OnTransparencyChange);
        sizeSlider.onValueChanged.AddListener(OnSizeChange);

        pauseButton.onClick.AddListener(PauseTime);
        resumeButton.onClick.AddListener(ResumeTime);
        saveSessionButton.onClick.AddListener(SaveSession);
    }

    public void OnTransparencyChange(float value)
    {
        manipulator.SetAlpha(value);
    }

    public void OnSizeChange(float value)
    {
        manipulator.SetPointSize(value);
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

    public void TestSize(float value)
    {
        manipulator.TestSize(value);
    }
}
