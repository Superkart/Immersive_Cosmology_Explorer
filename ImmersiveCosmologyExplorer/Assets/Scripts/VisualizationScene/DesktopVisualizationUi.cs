using UnityEngine;
using UnityEngine.UI;

public class DesktopVisualizationUI : MonoBehaviour
{
    [Header("References")]
    public DataManipulator manipulator;
    public SessionSaver sessionSaver;
    public NewDataImporter importer;

    [Header("Slider Controls")]
    public Slider sizeSlider;
    public Slider transparencySlider;
    public Slider filterMinSlider;
    public Slider filterMaxSlider;

    [Header("Buttons")]
    public Button playButton;
    public Button pauseButton;
    public Button saveButton;

    void Start()
    {
        if (sizeSlider != null)
            sizeSlider.onValueChanged.AddListener(v => manipulator.SetPointSize(v));

        if (transparencySlider != null)
            transparencySlider.onValueChanged.AddListener(v => manipulator.SetAlpha(v));

        if (filterMinSlider != null && filterMaxSlider != null)
        {
            filterMinSlider.onValueChanged.AddListener(v =>
                manipulator.SetFilter(filterMinSlider.value, filterMaxSlider.value));

            filterMaxSlider.onValueChanged.AddListener(v =>
                manipulator.SetFilter(filterMinSlider.value, filterMaxSlider.value));
        }

        if (playButton != null)
            playButton.onClick.AddListener(() => importer.Play());

        if (pauseButton != null)
            pauseButton.onClick.AddListener(() => importer.Pause());

        if (saveButton != null)
            saveButton.onClick.AddListener(() => sessionSaver.SaveSession());
    }
}
