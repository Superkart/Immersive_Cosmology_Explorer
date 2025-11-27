[System.Serializable]
public class SessionData
{
    public string sessionName;

    // Which dataset this session belongs to
    public string datasetId;

    // Player transform
    public float[] playerPos;   // length 3
    public float[] playerRot;   // length 4

    // Manipulation settings
    public float pointSize;
    public float alpha;
    public float filterMin;
    public float filterMax;
    public bool visibilityOn;
}
