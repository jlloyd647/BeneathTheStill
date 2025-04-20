using UnityEngine;

public class Bed : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TimeManager timeManager = FindAnyObjectByType<TimeManager>();
            if (timeManager != null)
            {
                timeManager.AdvanceTime(8f); // Skip 8 in-game hours
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}