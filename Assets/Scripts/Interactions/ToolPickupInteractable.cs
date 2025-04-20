using UnityEngine;

public class ToolPickupInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string toolName = "Fishing Rod";

    public void Interact(GameObject interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return;

        InventoryItem tool = new InventoryItem
        {
            itemName = toolName,
            itemType = ItemType.Tool,
            quantity = 1,
            stackable = false
        };

        if (Time.time - Inventory.lastToolInteractionTime < Inventory.toolInteractionCooldown) return;
        Inventory.lastToolInteractionTime = Time.time;

        Debug.Log($"Attempting to add {tool.itemName} to inventory.");
        if (inventory.AddItem(tool))
        {
            Debug.Log($"🛠️ You picked up the {toolName}!");
            gameObject.SetActive(false); // Hide pickup
            // If it's the fishing rod, enable fishing spots
            if (tool.itemName == "Fishing Rod")
            {
                Debug.Log("🎣 Enabling fishing spots...");
                foreach (FishingSpotEnabler spot in Object.FindObjectsByType<FishingSpotEnabler>(FindObjectsSortMode.None))
                {
                    spot.SetFishingEnabled(true);
                }
            }
        }
        else
        {
            Debug.Log("You're already holding too many tools.");
        }
    }
}
