using UnityEngine;

public class ChoppingStation : MonoBehaviour, IInteractable
{
    private TimedStationInteraction stationInteraction;
    private bool isReadyToChop = false;
    private Inventory cachedInventory;

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

        // This is called *after* the timer finishes
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
    }

    public float GetInteractDuration()
    {
        isReadyToChop = false;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return 0f;

        cachedInventory = player.GetComponent<Inventory>() ?? player.GetComponentInChildren<Inventory>();
        if (cachedInventory != null && cachedInventory.HasItem("Potato"))
        {
            cachedInventory.RemoveItem("Potato");

            // START the station timer, but DON'T call Interact() yet
            stationInteraction.StartInteraction(player, () =>
            {
                isReadyToChop = true;
                Interact(player); // ← Now we chop AFTER the bar completes
            });

            return 0f;
        }

        Debug.Log("❌ Player does not have a Potato.");
        return 0f;
    }

    public bool AllowMovementDuringInteract() => true;
}
