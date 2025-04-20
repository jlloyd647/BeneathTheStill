using UnityEngine;

public class CleanableDirt : MonoBehaviour, IInteractable
{
    private bool playerCanClean = false;

    public void Interact(GameObject interactor)
    {
        if (!playerCanClean)
        {
            Debug.Log("❌ You need a mop to clean this.");
            return;
        }

        Debug.Log("✨ Cleaned dirt!");
        Destroy(gameObject);
    }

    public float GetInteractDuration()
    {
        playerCanClean = false;

        Inventory inventory = TryGetInventory();
        if (inventory != null && HasTool(inventory, "Mop"))
        {
            playerCanClean = true;
            return 3f;
        }

        return 0f; // No bar, no interaction
    }

    public bool AllowMovementDuringInteract()
    {
        return false;
    }

    private Inventory TryGetInventory()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return null;

        return player.GetComponent<Inventory>() ?? player.GetComponentInChildren<Inventory>();
    }

    private bool HasTool(Inventory inventory, string toolName)
    {
        foreach (var tool in inventory.toolSlots)
        {
            if (tool != null && tool.itemName == toolName)
                return true;
        }

        return false;
    }
}
