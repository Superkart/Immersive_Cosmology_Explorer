using UnityEngine;

public class VisualizationSceneController : MonoBehaviour
{
    public NewDataImporter importer;
    public DataManipulator dataManipulator;

    public Transform playerRig;   // assign XR Rig or camera rig root in inspector

    void Start()
    {
        Debug.Log("Visualization Scene Started.");

        // 1. Need dataset!
        if (string.IsNullOrEmpty(SessionManager.Instance.selectedDataFolder))
        {
            Debug.LogError("No dataset path found! You must load a dataset first.");
            return;
        }

        // 2. Load the dataset
        importer.LoadDataFromFolder(SessionManager.Instance.selectedDataFolder);

        // 2.5 🔹 Apply manipulator defaults once clouds exist
        if (dataManipulator != null)
        {
            dataManipulator.ApplyFromCurrent();
        }

        // 3. Apply session if present (this can override defaults)
        if (SessionManager.Instance.loadedSessionData != null)
        {
            ApplySavedSession(SessionManager.Instance.loadedSessionData);
        }
    }


    private void ApplySavedSession(SessionData s)
    {
        if (s == null) return;

        Debug.Log("Applying saved session settings...");

        // ----------------------
        // APPLY PLAYER POSITION
        // ----------------------
        if (playerRig != null && s.playerPos != null && s.playerRot != null)
        {
            playerRig.position = new Vector3(
                s.playerPos[0],
                s.playerPos[1],
                s.playerPos[2]
            );

            playerRig.rotation = new Quaternion(
                s.playerRot[0],
                s.playerRot[1],
                s.playerRot[2],
                s.playerRot[3]
            );
        }

        // ----------------------
        // APPLY DATA MANIPULATION
        // ----------------------
        if (dataManipulator != null)
        {
            dataManipulator.SetPointSize(s.pointSize);
            dataManipulator.SetAlpha(s.alpha);
            dataManipulator.SetFilter(s.filterMin, s.filterMax);

            if (!s.visibilityOn)
                dataManipulator.ToggleVisibility();
        }

        Debug.Log("✔ Session restored!");
    }
}
