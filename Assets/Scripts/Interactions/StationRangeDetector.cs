using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StationRangeDetector : MonoBehaviour
{
    private TimedStationInteraction timedInteraction;

    private void Awake()
    {
        timedInteraction = GetComponent<TimedStationInteraction>();
        if (timedInteraction == null)
            Debug.LogError("❌ StationRangeDetector requires TimedStationInteraction!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timedInteraction.SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timedInteraction.SetPlayerInRange(false);
        }
    }
}
