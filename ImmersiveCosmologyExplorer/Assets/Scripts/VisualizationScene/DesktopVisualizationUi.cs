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
        // Correct callbacks for NEW shader
        transparencySlider.onValueChanged.AddListener(OnTransparencyChange);
        sizeSlider.onValueChanged.AddListener(OnSizeChange);

        pauseButton.onClick.AddListener(PauseTime);
        resumeButton.onClick.AddListener(ResumeTime);
        saveSessionButton.onClick.AddListener(SaveSession);
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
}
