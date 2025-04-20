using UnityEngine;

public class PlayerDebugTools : MonoBehaviour
{
    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (inventory.HasItem("Hammer"))
            {
                inventory.RemoveFromToolSlot("Hammer");
                Debug.Log("🛠️ Hammer unequipped (removed from inventory)");
            }
            else
            {
                InventoryItem hammer = new InventoryItem
                {
                    itemName = "Hammer",
                    itemType = ItemType.Tool,
                    quantity = 1,
                    stackable = false
                };
                inventory.AddItem(hammer);
                Debug.Log("🛠️ Hammer equipped (added to inventory)");
            }
        }
    }
}
