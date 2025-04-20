using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Image equippedToolImage;
    [SerializeField] private Sprite defaultToolSprite;

    [System.Serializable]
    public class ToolIconEntry
    {
        public string toolName;
        public Sprite icon;
    }

    [SerializeField] private List<ToolIconEntry> toolIcons;

    private Inventory inventory;

    public void SetInventory(Inventory inv)
    {
        if (inventory != null)
            inventory.OnToolChanged -= UpdateToolUI;

        inventory = inv;

        if (inventory != null)
        {
            inventory.OnToolChanged += UpdateToolUI;
            Debug.Log("[UIManager] Inventory set and subscribed.");
            UpdateToolUI(null);
        }
        else
        {
            Debug.LogWarning("[UIManager] Inventory is null in SetInventory.");
        }
    }

    private void Start()
    {
        Debug.Log("[UIManager] InventoryUIManager started. Waiting for SetInventory...");
    }

    private void UpdateToolUI(string toolName)
    {
        Debug.Log($"[UIManager] UpdateToolUI called with: {toolName ?? "NULL"}");

        if (string.IsNullOrEmpty(toolName))
        {
            Debug.Log("[UIManager] Showing default tool sprite (no tool equipped)");
            equippedToolImage.sprite = defaultToolSprite;
            equippedToolImage.enabled = true; // 🔥 THIS IS KEY
            return;
        }

        var match = toolIcons.FirstOrDefault(t => t.toolName == toolName);
        if (match == null)
        {
            Debug.LogWarning($"[UIManager] No matching icon for tool: {toolName}");
        }

        equippedToolImage.sprite = match?.icon ?? defaultToolSprite;
        equippedToolImage.enabled = true;
    }
}
