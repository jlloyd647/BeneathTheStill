using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Public lists for the Inspector
    public List<InventoryItem> generalItems = new();
    public List<InventoryItem> toolSlots = new();
    private int toolSlotCount = 1;
    public static float lastToolInteractionTime = -1f;
    public static float toolInteractionCooldown = 0.2f; // 200ms buffer
    // Event delegate for when the equipped tool changes
    public delegate void ToolChanged(string newToolName);
    public event ToolChanged OnToolChanged;

    private void Awake()
    {
        toolSlots.Clear(); // Ensure we're not stacking extra slots

        for (int i = 0; i < toolSlotCount; i++)
        {
            toolSlots.Add(null);
        }

        Debug.Log($"Initialized {toolSlotCount} tool slots.");
    }

    // Add any item to the correct list
    public bool AddItem(InventoryItem item)
    {
        if (item.itemType == ItemType.Tool)
        {
            return AddToToolSlot(item);
        }
        else
        {
            return AddToGeneralInventory(item);
        }
    }

    // Adds tools to the limited tool slot list
    private bool AddToToolSlot(InventoryItem item)
    {
        for (int i = 0; i < toolSlots.Count; i++)
        {
            if (toolSlots[i] == null)
            {
                toolSlots[i] = item;
                Debug.Log($"✅ Added {item.itemName} to tool slot {i}");
                OnToolChanged?.Invoke(item.itemName); // 🔔 Notify listeners
                return true;
            }
        }

        Debug.Log("❌ Tool inventory full!");
        return false;
    }

    // Adds stackable or new general items
    private bool AddToGeneralInventory(InventoryItem item)
    {
        if (item.stackable)
        {
            var existing = generalItems.FirstOrDefault(i => i.itemName == item.itemName);
            if (existing != null)
            {
                existing.quantity += item.quantity;
                return true;
            }
        }

        generalItems.Add(item);
        return true;
    }

    // Optional: Remove items by name
    public bool RemoveItem(string itemName, int amount = 1)
    {
        var item = generalItems.FirstOrDefault(i => i.itemName == itemName);
        if (item == null) return false;

        if (item.stackable)
        {
            item.quantity -= amount;
            if (item.quantity <= 0) generalItems.Remove(item);
        }
        else
        {
            generalItems.Remove(item);
        }

        return true;
    }

    public InventoryItem FindItemByName(string itemName)
    {
        return generalItems.FirstOrDefault(item => item.itemName == itemName && item.quantity > 0);
    }

    public InventoryItem FindIngredientBySubType(IngredientSubType subType)
    {
        return generalItems.FirstOrDefault(item =>
            item.itemType == ItemType.Ingredient &&
            item.ingredientSubType == subType &&
            item.quantity > 0);
    }

    public bool HasItem(string itemName)
    {
        // Check tool slots
        foreach (var tool in toolSlots)
        {
            if (tool != null && tool.itemName == itemName)
                return true;
        }

        // Check general inventory
        foreach (var item in generalItems)
        {
            if (item != null && item.itemName == itemName && item.quantity > 0)
                return true;
        }

        return false;
    }

    public bool RemoveFromToolSlot(string itemName)
    {
        for (int i = 0; i < toolSlots.Count; i++)
        {
            if (toolSlots[i] != null && toolSlots[i].itemName == itemName)
            {
                toolSlots[i] = null;
                Debug.Log($"🗑️ Removed {itemName} from tool slot {i}");

                OnToolChanged?.Invoke(null); // 🔔 Notify UI to show default
                return true;
            }
        }
        return false;
    }
}
