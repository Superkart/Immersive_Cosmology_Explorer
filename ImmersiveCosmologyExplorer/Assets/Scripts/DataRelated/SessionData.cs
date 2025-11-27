using UnityEngine;

[System.Serializable]
public class ManipulationState
{
    public float pointSize;
    public float alpha;
    public float filterMin;
    public float filterMax;
}

[System.Serializable]
public class PlayerState
{
    public Vector3 pos;
    public Vector3 rot;
}

[System.Serializable]
public class SessionData
{
    public string datasetId;

    public ManipulationState manipState;
    public PlayerState playerState;
}
