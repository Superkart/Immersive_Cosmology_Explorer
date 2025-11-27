using System.Collections.Generic;
using UnityEngine;

public class DataManipulator : MonoBehaviour
{
    [Header("References")]
    public NewDataImporter importer;

    // Internal cache of materials so we don't repeatedly search
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
                cachedMaterials.Add(mr.material);
            }
        }

        materialsCached = true;
        Debug.Log("[DataManipulator] Cached " + cachedMaterials.Count + " materials.");
    }

    // -------------------------------
    // POINT SIZE
    // -------------------------------

    public void SetPointSize(float size01)
    {
        CacheMaterials();

        float size = Mathf.Lerp(0.05f, 1.2f, size01);

        foreach (Material m in cachedMaterials)
        {
            if (m.HasProperty("_Size"))
            {
                m.SetFloat("_Size", size);
            }
        }
    }

    // -------------------------------
    // TRANSPARENCY
    // -------------------------------

    public void SetAlpha(float alpha01)
    {
        CacheMaterials();

        float a = Mathf.Clamp01(alpha01);

        foreach (Material m in cachedMaterials)
        {
            if (m.HasProperty("_Alpha"))
            {
                m.SetFloat("_Alpha", a);
            }
        }
    }

    // -------------------------------
    // FILTER RANGE
    // -------------------------------

    public void SetFilter(float minValue, float maxValue)
    {
        CacheMaterials();

        foreach (Material m in cachedMaterials)
        {
            if (m.HasProperty("_FilterMin"))
                m.SetFloat("_FilterMin", minValue);

            if (m.HasProperty("_FilterMax"))
                m.SetFloat("_FilterMax", maxValue);
        }
    }

    // -------------------------------
    // TOGGLE VISIBILITY
    // -------------------------------

    public void ToggleVisibility()
    {
        List<GameObject> clouds = importer.GetAllPointCloudObjects();
        bool newState = !clouds[0].activeSelf;

        foreach (GameObject go in clouds)
            go.SetActive(newState);
    }
}
