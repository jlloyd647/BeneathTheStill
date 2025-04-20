using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolTileSwapInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string toolName = "Fishing Rod";
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Vector3Int[] targetPositions;
    [SerializeField] private TileBase[] newTiles;

    private TileBase[] originalTiles;
    private bool toolGiven = false;

    private void Start()
    {
        // Cache original tiles so we can revert them later
        originalTiles = new TileBase[targetPositions.Length];
        for (int i = 0; i < targetPositions.Length; i++)
        {
            originalTiles[i] = tilemap.GetTile(targetPositions[i]);
        }
    }

    public void Interact(GameObject interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return;

        if (!toolGiven)
        {
            InventoryItem tool = new InventoryItem
            {
                itemName = toolName,
                itemType = ItemType.Tool,
                quantity = 1,
                stackable = false
            };

            if (Time.time - Inventory.lastToolInteractionTime < Inventory.toolInteractionCooldown) return;
            Inventory.lastToolInteractionTime = Time.time;

            if (inventory.AddItem(tool))
            {
                toolGiven = true;
                Debug.Log($"🛠️ {toolName} acquired via interaction point.");

                for (int i = 0; i < targetPositions.Length; i++)
                {
                    if (i < newTiles.Length && newTiles[i] != null)
                    {
                        tilemap.SetTile(targetPositions[i], newTiles[i]);
                    }
                }
                Debug.Log("🧱 Tiles updated.");
            }
            else
            {
                Debug.Log("❌ Inventory full — couldn't take tool.");
            }
        }
        else
        {
            // Player already has tool — try to return it
            if (inventory.HasItem(toolName))
            {
                if (inventory.RemoveFromToolSlot(toolName))
                {
                    toolGiven = false;
                    RevertTiles();
                    Debug.Log($"🔁 {toolName} returned and tiles reverted.");
                }
            }
        }
    }

    public void RevertTiles()
    {
        for (int i = 0; i < targetPositions.Length; i++)
        {
            if (i < originalTiles.Length && originalTiles[i] != null)
            {
                tilemap.SetTile(targetPositions[i], originalTiles[i]);
            }
        }
    }
}
