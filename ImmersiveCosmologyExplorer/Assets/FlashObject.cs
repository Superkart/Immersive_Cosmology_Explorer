using UnityEngine;

public class FlashObject : MonoBehaviour
{
    public GameObject target;
    public float duration = 2f;

    public void Flash()
    {
        if (target == null) return;

        target.SetActive(true);
        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), duration);
    }

    void Hide()
    {
        if (target == null) return;
        target.SetActive(false);
    }
}
