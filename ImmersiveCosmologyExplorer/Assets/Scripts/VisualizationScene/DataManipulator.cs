using System.Collections.Generic;
using UnityEngine;

public class DataManipulator : MonoBehaviour
{
    [Header("References")]
    public NewDataImporter importer;

    // -------------------------------
    // VALUES TO SAVE / RESTORE
    // -------------------------------
    [Range(0f, 1f)]
    public float currentPointSize = 0.2f;   // default a bit > 0 so we see something

    [Range(0f, 1f)]
    public float currentAlpha = 1f;

    public float currentFilterMin = 0f;
    public float currentFilterMax = 1f;

    public bool currentVisibility = true;

    // Internal cache so we don't repeatedly find materials
    private List<Material> cachedMaterials = new List<Material>();
    private bool materialsCached = false;

    private void CacheMaterials()
    {
        if (materialsCached || importer == null) return;

        List<GameObject> clouds = importer.GetAllPointCloudObjects();

        foreach (GameObject cloud in clouds)
        {
            MeshRenderer mr = cloud.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                // Clone material instance so each cloud is independent
                cachedMaterials.Add(mr.material);
            }
        }

        materialsCached = true;
        Debug.Log("[DataManipulator] Cached " + cachedMaterials.Count + " materials.");
    }

    // 🔹 NEW: convenience function to re-apply all current values
    public void ApplyFromCurrent()
    {
        // Guard: importer must have loaded data already
        if (importer == null) return;

        SetPointSize(currentPointSize);
        SetAlpha(currentAlpha);
        SetFilter(currentFilterMin, currentFilterMax);
    }

    // 🔹 NEW: when values change in inspector *during Play mode*, apply them
    private void OnValidate()
    {
        // OnValidate also runs in edit mode; only act when playing
        if (!Application.isPlaying) return;
        if (importer == null) return;

        ApplyFromCurrent();
    }

    // -------------------------------
    // POINT SIZE
    // -------------------------------
    public void SetPointSize(float size01)
    {
        CacheMaterials();

        currentPointSize = size01;

        float size = Mathf.Lerp(0.05f, 1.2f, size01);

        foreach (Material m in cachedMaterials)
            if (m.HasProperty("_Size"))
                m.SetFloat("_Size", size);
    }

    // -------------------------------
    // TRANSPARENCY
    // -------------------------------
    public void SetAlpha(float alpha01)
    {
        CacheMaterials();

        currentAlpha = alpha01;

        float a = Mathf.Clamp01(alpha01);

        foreach (Material m in cachedMaterials)
            if (m.HasProperty("_Alpha"))
                m.SetFloat("_Alpha", a);
    }

    // -------------------------------
    // FILTER RANGE
    // -------------------------------
    public void SetFilter(float minValue, float maxValue)
    {
        CacheMaterials();

        currentFilterMin = minValue;
        currentFilterMax = maxValue;

        foreach (Material m in cachedMaterials)
        {
            if (m.HasProperty("_FilterMin")) m.SetFloat("_FilterMin", minValue);
            if (m.HasProperty("_FilterMax")) m.SetFloat("_FilterMax", maxValue);
        }
    }

    // -------------------------------
    // TOGGLE VISIBILITY
    // -------------------------------
    public void ToggleVisibility()
    {
        List<GameObject> clouds = importer.GetAllPointCloudObjects();
        if (clouds.Count == 0) return;

        bool newState = !clouds[0].activeSelf;

        currentVisibility = newState;

        foreach (GameObject go in clouds)
            go.SetActive(newState);
    }


    public void TestSize(float value)
    {
        // Get all particle objects
        var clouds = importer.GetAllPointCloudObjects();
        if (clouds == null || clouds.Count == 0)
        {
            Debug.LogWarning("No point clouds found.");
            return;
        }

        Debug.Log("TestSize called: " + value);

        foreach (var cloud in clouds)
        {
            var mr = cloud.GetComponent<MeshRenderer>();
            if (mr == null) continue;

            var mat = mr.material;

            // We test with "_Radius" because your current shader exposes it
            if (mat.HasProperty("_Radius"))
            {
                float radius = Mathf.Lerp(0.01f, 0.2f, value);
                mat.SetFloat("_Radius", radius);
            }
            else
                Debug.LogWarning("Material has no _Radius property!");
        }
    }

}
