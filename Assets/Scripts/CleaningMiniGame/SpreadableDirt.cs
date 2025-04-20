using UnityEngine;

public class SpreadableDirt : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"TRIGGER HIT: {other.name}");

        if (other.CompareTag("Player") || other.CompareTag("Crew"))
        {
            Debug.Log($"{other.name} is valid to spread dirt.");
            var tracker = other.GetComponent<DirtTracker>();
            if (tracker != null)
            {
                tracker.GetDirty(2);
            }
        }
    }
}
