using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class NewDataImporter : MonoBehaviour
{
    public GameObject parentPrefab;
    public TextMeshProUGUI timestepText;

    private string dataFolderPath = "";

    public List<GameObject> timeStepParents = new List<GameObject>();
    public List<GameObject> pointCloudObjects = new List<GameObject>();

    public int currentTimestepIndex = 0;
    public float timer;
    public bool isTimeLapseActive = false;
    public float delayBetweenTimeSteps = 1.0f;
    public int totalTimesteps;
    public bool isLooping = false;
    public bool arePointCloudsVisible = true;

    public List<bool> particleVisibility = new List<bool>();
    public List<int> frameNumbers = new List<int>();
    public List<int> allFrames = new List<int>();


    // --- NEW METHOD: Called by Desktop UI ---
    public void LoadDataFromFolder(string folderPath)
    {
        Debug.Log("Loading data from: " + folderPath);

        dataFolderPath = folderPath;

        LoadFrameNumbersFromDirectory();

        if (allFrames.Count == 0)
        {
            Debug.LogError("No CSV data found in folder.");
            if (timestepText != null) timestepText.text = "No data found";
            return;
        }

        totalTimesteps = allFrames.Count;

        CreateTimeStepParents();
        ImportData();
        SetActiveTimeStepNew(currentTimestepIndex, true);
        StartAutoPlay();
    }


    private void LoadFrameNumbersFromDirectory()
    {
        if (!Directory.Exists(dataFolderPath))
        {
            Debug.LogError("Directory NOT found: " + dataFolderPath);
            return;
        }

        Regex regex = new Regex(@"m000p\.particles-(\d+)\.csv$");
        HashSet<int> uniqueFrames = new HashSet<int>();

        foreach (string filePath in Directory.GetFiles(dataFolderPath, "m000p.particles-*.csv"))
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

        Debug.Log($"Found {allFrames.Count} timesteps.");
    }


    private void CreateTimeStepParents()
    {
        foreach (GameObject old in timeStepParents)
            Destroy(old);

        timeStepParents.Clear();

        for (int i = 0; i < totalTimesteps; i++)
        {
            GameObject timeStepParent = Instantiate(parentPrefab);
            timeStepParent.name = $"TimeStep_{allFrames[i]}";
            timeStepParents.Add(timeStepParent);
        }
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

            timeStepObjectsParent.transform.parent = timeStepParents[parentIndex].transform;
            timeStepParents[parentIndex].SetActive(false);
        }
    }


    private void ImportPointCloud(int frameNum, GameObject parent)
    {
        string filePath = Path.Combine(dataFolderPath, $"m000p.particles-{frameNum:D4}.csv");

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Missing file: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        int n = lines.Length - 1;
        Vector3[] points = new Vector3[n];
        Color[] colors = new Color[n];
        int[] indices = new int[n];
        float[] scalarNum = new float[n];

        float minScalar = float.MaxValue;
        float maxScalar = float.MinValue;

        for (int i = 0; i < n; i++)
        {
            string[] vals = lines[i + 1].Split(',');
            float x = float.Parse(vals[2]);
            float y = float.Parse(vals[3]);
            float z = float.Parse(vals[4]);
            float scalar = float.Parse(vals[0]);

            scalarNum[i] = scalar;
            points[i] = new Vector3(x, y, z);

            if (scalar < minScalar) minScalar = scalar;
            if (scalar > maxScalar) maxScalar = scalar;
        }

        for (int i = 0; i < n; i++)
        {
            colors[i] = Color.Lerp(Color.red, Color.yellow,
                (scalarNum[i] - minScalar) / (maxScalar - minScalar));
            colors[i].a = 0.5f;

            indices[i] = i;
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);


        GameObject obj = new GameObject("HydroParticles_" + frameNum);
        Debug.Log("CREATED: " + obj.name + " under parent: " + parent.name);

        obj.AddComponent<MeshFilter>().mesh = mesh;
        obj.AddComponent<MeshRenderer>().material = Resources.Load<Material>("PointCloud");

        // 🟩 FIX — Store the object so DataManipulator can find it
        pointCloudObjects.Add(obj);

        obj.transform.parent = parent.transform;
    }


    private void SetActiveTimeStep(int index, bool active = true)
    {
        for (int i = 0; i < timeStepParents.Count; i++)
            timeStepParents[i].SetActive(i == index && active);

        if (timestepText != null)
            timestepText.text = "Timestep: " + frameNumbers[index];
    }

    private void SetActiveTimeStepNew(int index, bool active = true)
    {
        Debug.Log("🔵 SetActiveTimeStep called. Index = " + index + "  Active = " + active);

        if (index < 0 || index >= timeStepParents.Count)
        {
            Debug.LogError("❌ Invalid timestep index: " + index);
            return;
        }

        for (int i = 0; i < timeStepParents.Count; i++)
        {
            bool shouldBeActive = (i == index && active);
            timeStepParents[i].SetActive(shouldBeActive);

            Debug.Log("   • TimeStep_" + i + " → " + (shouldBeActive ? "ENABLED" : "disabled"));
        }
    }


    private void StartAutoPlay()
    {
        timer = 0f;
        isTimeLapseActive = true;
        isLooping = true;
        Debug.Log("Timeline autoplay started");
    }

    private void Update()
    {
        if (!isTimeLapseActive) return;

        timer += Time.deltaTime;

        if (timer >= delayBetweenTimeSteps)
        {
            timer = 0f;
            AdvanceTimeStep();
        }
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
            return;
        }

        SetActiveTimeStep(currentTimestepIndex, true);
    }

    public List<GameObject> GetAllPointCloudObjects()
    {
        return pointCloudObjects;
    }

    public void Pause()
    {
        isTimeLapseActive = false;
    }

    public void Resume()
    {
        isTimeLapseActive = true;
    }


}
