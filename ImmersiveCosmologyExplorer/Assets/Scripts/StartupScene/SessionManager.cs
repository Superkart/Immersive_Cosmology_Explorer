using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    [Header("Dataset Info")]
    public string selectedDataFolder = "";
    public string currentDatasetID = "";

    [Header("Session Info")]
    public SessionData loadedSessionData = null;
    public string selectedSessionName = "";

    void Awake()
    {
        // enforce singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
