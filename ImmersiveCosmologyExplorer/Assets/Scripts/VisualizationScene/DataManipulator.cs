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

            Material mat = mr.material;   // instance

            action(mat);
        }
    }

    // ----------------------------------------------------
    // SIZE
    // ----------------------------------------------------
    public void SetPointSize(float size01)
    {
        currentPointSize = size01;

        float size = Mathf.Lerp(0.05f, 1.2f, size01);

        ForEachMaterial(mat =>
        {
            if (mat.HasProperty("_Size"))
                mat.SetFloat("_Size", size);

            if (mat.HasProperty("_Radius"))
                mat.SetFloat("_Radius", size * 0.1f); // adjust for your shader
        });
    }

    // ----------------------------------------------------
    // ALPHA
    // ----------------------------------------------------
    public void SetAlpha(float alpha01)
    {
        currentAlpha = alpha01;
        float a = Mathf.Clamp01(alpha01);

        ForEachMaterial(mat =>
        {
            if (mat.HasProperty("_Alpha"))
                mat.SetFloat("_Alpha", a);

            if (mat.HasProperty("_Transparency"))
                mat.SetFloat("_Transparency", 1f - a);
        });
    }

    // ----------------------------------------------------
    // FILTER
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
    // VISIBILITY
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

    // ----------------------------------------------------
    // Test for Shader Radius
    // ----------------------------------------------------

    public void SetSphereRadius(float radius)
    {
        ForEachMaterial(mat =>
        {
            if (mat.HasProperty("_Radius"))
            {
                mat.SetFloat("_Radius", radius);
                Debug.Log("Sphere radius set to: " + radius);
            }
            else
            {
                Debug.LogWarning("Material does NOT have _Radius property");
            }
        });
    }


}
