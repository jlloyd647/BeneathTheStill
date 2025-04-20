using UnityEngine;

public class FishingSpotEnabler : MonoBehaviour
{
    [SerializeField] private Collider2D interactionCollider;

    void Start()
    {
        Debug.Log("FishingSpotEnabler started.");
        if (interactionCollider != null)
            interactionCollider.enabled = false; // Always start disabled
    }
    public void SetFishingEnabled(bool enabled)
    {
        Debug.Log($"Fishing enabled: {enabled}");
        if (interactionCollider != null)
            interactionCollider.enabled = enabled;
    }
}
