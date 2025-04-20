using UnityEngine;

public class ToolReturnZone : MonoBehaviour
{
    [SerializeField] private string toolName = "Fishing Rod";
    [SerializeField] private ToolTileSwapInteractable swapReference;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory == null) return;

        if (inventory.HasItem(toolName)) // You'll need to implement HasItem in your inventory
        {
            inventory.RemoveItem(toolName); // And this too
            Debug.Log($"📤 {toolName} returned.");
            swapReference.RevertTiles();
        }
    }
}