using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private InventoryUIManager inventoryUIManager;

    private void Start()
    {
        GameObject spawnPoint = GameObject.Find(SpawnManager.NextSpawnPointName);

        if (spawnPoint != null)
            {
                GameObject player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

                // Update camera target
                CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
                if (camFollow != null)
                {
                    camFollow.target = player.transform;
                }

                // 🔗 Hook up inventory
                Inventory playerInventory = player.GetComponent<Inventory>();
                if (playerInventory != null && inventoryUIManager != null)
                {
                    inventoryUIManager.SetInventory(playerInventory);
                }
            }
        else
        {
            Debug.LogWarning("Spawn point not found: " + SpawnManager.NextSpawnPointName);
        }
    }
}
