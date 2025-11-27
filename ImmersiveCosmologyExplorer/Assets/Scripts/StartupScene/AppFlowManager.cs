using UnityEngine;
using UnityEngine.SceneManagement;

public class AppFlowManager : MonoBehaviour
{
    public void StartVisualizationScene()
    {
        SceneManager.LoadScene(1);
    }
}
