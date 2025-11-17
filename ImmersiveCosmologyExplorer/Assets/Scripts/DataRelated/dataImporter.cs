using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;

public class dataImporter : MonoBehaviour
{
    public GameObject parentPrefab; // create a public parent prefab and drag into a control empty game object
    public TextMeshProUGUI timestepText;

    private List<GameObject> timeStepParents = new List<GameObject>();
    private List<GameObject> pointCloudObjects = new List<GameObject>();

    // Time Series Control Fields
    private int currentTimestepIndex = 0;
    private float timer;
    private bool isTimeLapseActive = false;
    public float delayBetweenTimeSteps = 1.0f;
    private int totalTimesteps;
    private bool isLooping = false;
    private bool arePointCloudsVisible = true;

    // Data Containers
    private List<bool> particleVisibility = new List<bool>();
    private List<int> frameNumbers = new List<int>(); // Used for UI display
    private List<int> allFrames = new List<int>();    // Used for iteration

    private void Start()
    {
        LoadFrameNumbersFromDirectory();

        if (allFrames.Count == 0)
        {
            Debug.LogError("No point cloud data found. Initialization aborted.");
            if (timestepText != null) timestepText.text = "Error: No data found";
            return;
        }

        totalTimesteps = allFrames.Count;
        CreateTimeStepParents();
        ImportData();
        SetActiveTimeStep(currentTimestepIndex, true);
    }

    void Update()
    {
        if (isTimeLapseActive)
        {
            timer += Time.deltaTime;

            if (timer >= delayBetweenTimeSteps)
            {
                timer = 0f;
                AdvanceTimeStep();
            }
        }
    }

 
    private void LoadFrameNumbersFromDirectory()
    {
        // file path
        string directoryPath = "Assets/PARTIAL_PARTICLES_02";

        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"Directory not found: {directoryPath}");
            return;
        }
        // if you change the file path ensure you update the file name here too
        Regex regex = new Regex(@"m000p\.particles-(\d+)\.csv$");
        HashSet<int> uniqueFrames = new HashSet<int>();

        // here as well
        foreach (string filePath in Directory.GetFiles(directoryPath, "m000p.particles-*.csv"))
        {
            string fileName = Path.GetFileName(filePath);
            Match match = regex.Match(fileName);

            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out int frameNum))
                {
                    uniqueFrames.Add(frameNum);
                }
            }
        }

        allFrames = uniqueFrames.ToList();
        allFrames.Sort();
        frameNumbers = new List<int>(allFrames);

        Debug.Log($"Found {allFrames.Count} timesteps dynamically.");
    }

    private void CreateTimeStepParents()
    {
        for (int i = 0; i < totalTimesteps; i++)
        {
            GameObject timeStepParent = Instantiate(parentPrefab);
            timeStepParent.name = $"TimeStep_{allFrames[i]}";
            timeStepParents.Add(timeStepParent);
        }
    }

    private GameObject[] GetChildren(GameObject parent)
    {
        GameObject[] children = new GameObject[parent.transform.childCount];
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i).gameObject;
        }
        return children;
    }

    private void ImportData()
    {
        for (int i = 0; i < allFrames.Count; i++)
        {
            int frameNum = allFrames[i];
            int parentIndex = i;

            GameObject timeStepObjectsParent = Instantiate(parentPrefab);
            timeStepObjectsParent.name = $"FrameData_{frameNum}";

            GameObject pointCloudParent = new GameObject("Particle Data");
            pointCloudParent.transform.parent = timeStepObjectsParent.transform;

            ImportPointCloud(frameNum, pointCloudParent);

            // Attach the frame data parent to the time series parent
            timeStepObjectsParent.transform.parent = timeStepParents[parentIndex].transform;
            timeStepParents[parentIndex].SetActive(false);

            // Collect references
            pointCloudObjects.AddRange(GetChildren(pointCloudParent));
            particleVisibility.Add(arePointCloudsVisible);
        }
    }

    private void ImportPointCloud(int frameNum, GameObject parent)
    {
        // update the file path here
        string filePath = $"Assets/PARTIAL_PARTICLES_02/m000p.particles-{frameNum:D4}.csv";
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Particle Files for frame number {frameNum} not found at {filePath}");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        //int n = Mathf.Min(800000, lines.Length - 1); // clamp for the amount of particles loaded to avoid lags
        int n = lines.Length - 1; // use this for full load
        Vector3[] points = new Vector3[n];
        Color[] colors = new Color[n];
        int[] indices = new int[n];
        float[] scalarNum = new float[n];
        float minScalarNum = float.MaxValue;
        float maxScalarNum = float.MinValue;

        for (int i = 0; i < n; i++)
        {
            string[] values = lines[i + 1].Split(',');
            float x = float.Parse(values[2]);
            float y = float.Parse(values[3]);
            float z = float.Parse(values[4]);
            scalarNum[i] = float.Parse(values[0]);
            points[i] = new Vector3(x, y, z);
        }

        Color minScalarColor = new Color(1, 0, 0); // Red
        Color maxScalarColor = new Color(1, 1, 0); // Yellow

        // Recalculate min/max for accurate remapping
        for (int i = 0; i < n; i++)
        {
            minScalarNum = Mathf.Min(minScalarNum, scalarNum[i]);
            maxScalarNum = Mathf.Max(maxScalarNum, scalarNum[i]);
        }

        for (int i = 0; i < n; i++)
        {
            colors[i] = Color.Lerp(minScalarColor, maxScalarColor, remap(minScalarNum, maxScalarNum, 0, 1, scalarNum[i]));
            colors[i].a = 0.5f; // Set default alpha
            indices[i] = i;
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        GameObject pointCloudObject = new GameObject("HydroParticles_" + frameNum);

        MeshFilter meshFilter = pointCloudObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = pointCloudObject.AddComponent<MeshRenderer>();

        Material pointCloudMaterial = Resources.Load<Material>("PointCloud");

        meshRenderer.material = pointCloudMaterial;
        pointCloudObject.transform.parent = parent.transform;
    }

    // Helper function for color remapping
    private float remap(float a, float b, float c, float d, float x)
    {
        return c + (x - a) * (d - c) / (b - a);
    }

    // --- Time Series and UI Control Methods ---

    private void SetActiveTimeStep(int index, bool active = true)
    {
        if (index < 0 || index >= timeStepParents.Count)
        {
            Debug.LogWarning("Index out of range in SetActiveTimeStep");
            return;
        }

        for (int i = 0; i < timeStepParents.Count; i++)
        {
            timeStepParents[i].SetActive(i == index && active);
        }

        if (timestepText != null && frameNumbers.Count > index)
        {
            timestepText.text = $"Timestep: {frameNumbers[index]}";
        }
    }

    public void TogglePointCloudVisibility()
    {
        arePointCloudsVisible = !arePointCloudsVisible;
        foreach (GameObject pointCloud in pointCloudObjects)
        {
            pointCloud.SetActive(arePointCloudsVisible);
        }
    }

    public void PlayButton()
    {
        isTimeLapseActive = true;
        if (timestepText != null) timestepText.text = $"Timestep: {frameNumbers[currentTimestepIndex]}";
    }

    public void PauseButton()
    {
        isTimeLapseActive = false;
        if (timestepText != null) timestepText.text = $"Timestep: {frameNumbers[currentTimestepIndex]}";
    }

    public void NextButton()
    {
        if (currentTimestepIndex < totalTimesteps - 1)
        {
            SetActiveTimeStep(currentTimestepIndex, false);
            currentTimestepIndex++;
            SetActiveTimeStep(currentTimestepIndex, true);
        }
    }

    public void PreviousButton()
    {
        if (currentTimestepIndex > 0)
        {
            SetActiveTimeStep(currentTimestepIndex, false);
            currentTimestepIndex--;
            SetActiveTimeStep(currentTimestepIndex, true);
        }
    }

    public void LoopButton()
    {
        isLooping = !isLooping;
    }

    private void AdvanceTimeStep()
    {
        SetActiveTimeStep(currentTimestepIndex, false);

        if (currentTimestepIndex < totalTimesteps - 1)
        {
            currentTimestepIndex++;
        }
        else if (isLooping)
        {
            currentTimestepIndex = 0;
        }
        else
        {
            isTimeLapseActive = false; 
        }

        SetActiveTimeStep(currentTimestepIndex, true);
    }

    public void RefreshButton()
    {
        if (isTimeLapseActive)
        {
            PauseButton();
        }
        SetActiveTimeStep(currentTimestepIndex, false);
        currentTimestepIndex = 0;
        SetActiveTimeStep(currentTimestepIndex, true);
    }
}