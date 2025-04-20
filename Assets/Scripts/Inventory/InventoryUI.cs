using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel; // Panel that contains the inventory UI
    [SerializeField] private Text inventoryText;         // Text component to show inventory contents

    [Header("Player Reference")]
    [SerializeField] private Inventory playerInventory;  // Reference to player's Inventory component

    private bool isVisible = false;

    void Start()
    {
        if (playerInventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerInventory = player.GetComponent<Inventory>();
            }

            if (playerInventory == null)
            {
                Debug.LogWarning("⚠️ Could not find Inventory component on tagged Player.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isVisible = !isVisible;
            inventoryPanel.SetActive(isVisible);

            if (isVisible)
            {
                UpdateInventoryDisplay();
            }
        }
    }

    private void UpdateInventoryDisplay()
    {
        if (playerInventory == null)
        {
            inventoryText.text = "⚠️ Player inventory not assigned.";
            return;
        }

        StringBuilder sb = new StringBuilder();

        // Tool Slots
        sb.AppendLine("🧰 Tool Slots:");
        Debug.Log(playerInventory.toolSlots.Count);
        for (int i = 0; i < playerInventory.toolSlots.Count; i++)
        {
            var tool = playerInventory.toolSlots[i];
            string toolName = (tool != null && !string.IsNullOrEmpty(tool.itemName)) ? tool.itemName : "Empty";
            sb.AppendLine($"Slot {i + 1}: {toolName}");

            Debug.Log($"📦 UI Tool Slot {i}: {(tool != null ? tool.itemName : "null")}");
        }

        // General Inventory
        sb.AppendLine("\n🎒 General Inventory:");
        if (playerInventory.generalItems.Count == 0)
        {
            sb.AppendLine("Empty");
        }
        else
        {
            foreach (var item in playerInventory.generalItems)
            {
                sb.AppendLine($"{item.itemName} x{item.quantity}");
            }
        }

        inventoryText.text = sb.ToString();
        Debug.Log("✅ Inventory panel refreshed.");
    }
}
