using System.Collections.Generic;
using UnityEngine;

public class DataManipulator : MonoBehaviour
{
    [Header("References")]
    public NewDataImporter importer;

    [Range(0f, 1f)]
    public float currentPointSize = 0.2f;

    [Range(0f, 1f)]
    public float currentAlpha = 1f;

    public float currentFilterMin = 0f;
    public float currentFilterMax = 1f;

    public bool currentVisibility = true;

    // ----------------------------------------------------
    // Helper: Apply settings to all materials in all clouds
    // ----------------------------------------------------
    private void ForEachMaterial(System.Action<Material> action)
    {
        var clouds = importer?.GetAllPointCloudObjects();
        if (clouds == null) return;

        foreach (var cloud in clouds)
        {
            var mr = cloud.GetComponent<MeshRenderer>();
            if (mr == null) continue;

            Material mat = mr.material; // instance
            action(mat);
        }
    }

    // ----------------------------------------------------
    // POINT SIZE (NEW SHADER)
    // ----------------------------------------------------
    public void SetPointSize(float value)
    {
        currentPointSize = value;

        ForEachMaterial(mat =>
        {
            if (mat.HasProperty("_Size"))
                mat.SetFloat("_Size", value);
            else
                Debug.LogWarning("Material does NOT have _PointSize");
        });
    }

    // ----------------------------------------------------
    // ALPHA (NEW SHADER)
    // ----------------------------------------------------
    public void SetAlpha(float value)
    {
        currentAlpha = value;

        ForEachMaterial(mat =>
        {
            if (mat.HasProperty("_Alpha"))
                mat.SetFloat("_Alpha", value);
            else
                Debug.LogWarning("Material does NOT have _GlobalAlpha");
        });
    }

    // ----------------------------------------------------
    // FILTER RANGE (NEW SHADER)
    // ----------------------------------------------------
    public void SetFilter(float minValue, float maxValue)
    {
        currentFilterMin = minValue;
        currentFilterMax = maxValue;

        ForEachMaterial(mat =>
        {
            if (mat.HasProperty("_FilterMin"))
                mat.SetFloat("_FilterMin", minValue);

            if (mat.HasProperty("_FilterMax"))
                mat.SetFloat("_FilterMax", maxValue);
        });
    }

    // ----------------------------------------------------
    // VISIBILITY (unchanged)
    // ----------------------------------------------------
    public void ToggleVisibility()
    {
        var clouds = importer?.GetAllPointCloudObjects();
        if (clouds == null || clouds.Count == 0) return;

        bool newState = !clouds[0].activeSelf;
        currentVisibility = newState;

        foreach (var cloud in clouds)
            cloud.SetActive(newState);
    }
}
