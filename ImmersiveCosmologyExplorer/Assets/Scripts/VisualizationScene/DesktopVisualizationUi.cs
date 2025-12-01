using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Filter Inputs")]
    public TMP_InputField minFilterInput;
    public TMP_InputField maxFilterInput;
    public TextMeshProUGUI dataRangeText;

    [Header("Filter Buttons")]
    public Button applyFilterButton;
    public Button resetFilterButton;

    private float globalMinScalar = float.MaxValue;
    private float globalMaxScalar = float.MinValue;
    private float pendingMinFilter = 0f;
    private float pendingMaxFilter = 1f;

    void Start()
    {
        // Correct callbacks for NEW shader
        transparencySlider.onValueChanged.AddListener(OnTransparencyChange);
        sizeSlider.onValueChanged.AddListener(OnSizeChange);

        pauseButton.onClick.AddListener(PauseTime);
        resumeButton.onClick.AddListener(ResumeTime);
        saveSessionButton.onClick.AddListener(SaveSession);


        Debug.Log($"[Filter] Global Data Range Found: Min={globalMinScalar:F3}, Max={globalMaxScalar:F3}");
        if (dataRangeText != null)
        {
            dataRangeText.text = $"Data Range: Min {globalMinScalar:F3} to Max {globalMaxScalar:F3}";
        }
        pendingMinFilter = globalMinScalar;
        pendingMaxFilter = globalMaxScalar;

        if (minFilterInput != null) minFilterInput.text = "";
        if (maxFilterInput != null) maxFilterInput.text = "";

        ApplyFilter();

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




    public void StoreFilterValue(string newValue)
    {
        float min = globalMinScalar;
        float max = globalMaxScalar;

        // Use global values if input fields are empty or invalid
        // Parse Min Value
        if (minFilterInput != null && float.TryParse(minFilterInput.text, out float parsedMin))
        {
            min = parsedMin;
        }

        // Parse Max Value
        if (maxFilterInput != null && float.TryParse(maxFilterInput.text, out float parsedMax))
        {
            max = parsedMax;
        }

        // Ensure min <= max logic
        if (min > max)
        {
            pendingMinFilter = max;
            pendingMaxFilter = min;
        }
        else
        {
            pendingMinFilter = min;
            pendingMaxFilter = max;
        }
    }

    public void ApplyFilter()
    {
        StoreFilterValue("");
        foreach (var m in pointCloudMaterials)
        {
            if (m && m.HasProperty("_FilterMin")) m.SetFloat("_FilterMin", pendingMinFilter);
            if (m && m.HasProperty("_FilterMax")) m.SetFloat("_FilterMax", pendingMaxFilter);
        }

        Debug.Log($"[Filter Applied] Range: Min={pendingMinFilter:F3}, Max={pendingMaxFilter:F3}");
    }

    public void ResetFilter()
    {
        pendingMinFilter = globalMinScalar;
        pendingMaxFilter = globalMaxScalar;

        if (minFilterInput != null) minFilterInput.text = "";
        if (maxFilterInput != null) maxFilterInput.text = "";

        ApplyFilter();
    }

}
