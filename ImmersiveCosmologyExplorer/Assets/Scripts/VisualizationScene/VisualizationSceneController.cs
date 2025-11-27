using UnityEngine;

public class VisualizationSceneController : MonoBehaviour
{
    public NewDataImporter importer;
    public DataManipulator dataManipulator;

    void Start()
    {
        Debug.Log("Visualization Scene Started.");

        // 1. Make sure dataset path exists
        if (string.IsNullOrEmpty(SessionManager.Instance.selectedDataFolder))
        {
            Debug.LogError("No dataset path found! You must load a dataset first.");
            return;
        }

        // 2. Load the dataset
        importer.LoadDataFromFolder(SessionManager.Instance.selectedDataFolder);

        // 3. If a session exists → apply saved parameters
        if (SessionManager.Instance.loadedSessionData != null)
        {
            ApplySavedSession(SessionManager.Instance.loadedSessionData);
        }
    }

    private void ApplySavedSession(SessionData session)
    {
        if (session == null)
            return;

        Debug.Log("Applying saved session settings...");

        // Restore manipulator values  
        if (session.manipState != null)
        {
            dataManipulator.SetPointSize(session.manipState.pointSize);
            dataManipulator.SetAlpha(session.manipState.alpha);
            dataManipulator.SetFilter(session.manipState.filterMin,
                                      session.manipState.filterMax);
        }

        // Restore player  
        if (session.playerState != null)
        {
            if (Camera.main != null)
            {
                Camera.main.transform.position = session.playerState.pos;
                Camera.main.transform.rotation = Quaternion.Euler(session.playerState.rot);
            }
        }

        Debug.Log("✔ Session restored!");
    }

}
