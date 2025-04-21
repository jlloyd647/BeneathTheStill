using System.Collections.Generic;
using UnityEngine;

public class QuickSelectTestStation : MonoBehaviour
{
    public QuickSelectManager quickSelectManager;
    public List<InventoryItem> testItems;
    public string stationId = "test_station";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var lastUsed = LastUsedTracker.Get(stationId);
            quickSelectManager.Show(testItems, stationId, lastUsed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            quickSelectManager.Hide();
        }
    }
}
