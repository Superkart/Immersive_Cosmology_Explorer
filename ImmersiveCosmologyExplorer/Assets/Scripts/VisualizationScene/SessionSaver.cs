using UnityEngine;
using System.IO;

public class SessionSaver : MonoBehaviour
{
    [Header("References")]
    public DataManipulator manipulator;
    public Transform playerRig;
    public FlashObject flashObject ;

    private string sessionsFolder;

    void Start()
    {
        sessionsFolder = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
            "ICE_Sessions");

        if (!Directory.Exists(sessionsFolder))
            Directory.CreateDirectory(sessionsFolder);
    }



    public void SaveSession()
    {
        SessionData data = new SessionData();

        data.sessionName = "Session_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        data.datasetId = SessionManager.Instance.currentDatasetID;

        // Player
        data.playerPos = new float[]
        {
            playerRig.position.x,
            playerRig.position.y,
            playerRig.position.z
        };

        Quaternion q = playerRig.rotation;
        data.playerRot = new float[] { q.x, q.y, q.z, q.w };

        // Manipulation values
        data.pointSize = manipulator.currentPointSize;
        data.alpha = manipulator.currentAlpha;
        data.filterMin = manipulator.currentFilterMin;
        data.filterMax = manipulator.currentFilterMax;
        data.visibilityOn = manipulator.currentVisibility;

        string json = JsonUtility.ToJson(data, true);

        string filePath = Path.Combine(sessionsFolder, data.sessionName + ".json");
        File.WriteAllText(filePath, json);

        Debug.Log("✔ Session saved: " + filePath);
        flashObject.Flash();
    }
}
