using UnityEngine;

public class ChoppingStation : MonoBehaviour, IInteractable
{
    private TimedStationInteraction stationInteraction;
    private bool isReadyToChop = false;
    private bool isChopping = false;
    private Inventory cachedInventory;

    [SerializeField] private QuickSelectManager quickSelectManager;
    [SerializeField] private string stationId = "chopping_station";

    private void Awake()
    {
        stationInteraction = GetComponent<TimedStationInteraction>();
        if (stationInteraction == null)
            Debug.LogError("❌ TimedStationInteraction not found on ChoppingStation!");
    }

    public void Interact(GameObject interactor)
    {
        if (!isReadyToChop)
        {
            Debug.Log("❌ You need a Potato to chop.");
            return;
        }

        string outputItem = "Chopped Potato";

        InventoryItem choppedItem = new InventoryItem
        {
            itemName = outputItem,
            itemType = ItemType.Ingredient,
            quantity = 1,
            stackable = true
        };

        cachedInventory?.AddItem(choppedItem);
        Debug.Log($"🔪 Chopped Potato -> {outputItem}");

        isChopping = false; // ✅ Safely reset here
    }

    public float GetInteractDuration()
    {
        if (isChopping)
        {
            Debug.Log("🚫 Already chopping — wait until the current chop is done.");
            return 0f;
        }

        isReadyToChop = false;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return 0f;

        cachedInventory = player.GetComponent<Inventory>() ?? player.GetComponentInChildren<Inventory>();
        if (cachedInventory != null && cachedInventory.HasItem("Potato"))
        {
            cachedInventory.RemoveItem("Potato");
            isChopping = true;

            stationInteraction.StartInteraction(player, () =>
            {
                isReadyToChop = true;
                Interact(player); // ✅ will reset isChopping inside Interact
            });

            return 0f;
        }

        Debug.Log("❌ Player does not have a Potato.");
        return 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("Player has no Inventory component.");
            return;
        }

        // Filter for Fish and Vegetable
        var validItems = inventory.GetItemsByType(ItemType.Fish, IngredientSubType.Vegetable);
        if (validItems == null || validItems.Count == 0)
        {
            Debug.Log("No valid items to chop.");
            return;
        }

        var lastUsed = LastUsedTracker.Get(stationId);
        if (quickSelectManager == null)
        {
            Debug.LogError("❌ quickSelectManager is NULL — did you forget to assign it in the Inspector?");
            return;
        }

        quickSelectManager.Show(validItems, stationId, lastUsed, HandleItemSelected);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            quickSelectManager.Hide();
        }
    }

    private void HandleItemSelected(InventoryItem selected)
    {
        cachedInventory = GameObject.FindWithTag("Player")?.GetComponent<Inventory>();
        if (cachedInventory == null) return;

        string resultName = selected.itemType == ItemType.Fish
            ? "Fillet"
            : "Chopped " + selected.itemName;

        cachedInventory.RemoveItem(selected.itemName);

        InventoryItem resultItem = new InventoryItem
        {
            itemName = resultName,
            itemType = ItemType.Ingredient,
            quantity = 1,
            stackable = true,
            ingredientSubType = IngredientSubType.Cooked // or preserve original if needed
        };

        cachedInventory.AddItem(resultItem);
        Debug.Log($"🔪 Used {selected.itemName}, created {resultItem.itemName}");

        LastUsedTracker.Save(stationId, selected);
    }

    public bool AllowMovementDuringInteract() => true;
}
