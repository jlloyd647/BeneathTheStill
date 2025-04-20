using UnityEngine;

public class ToolReturnInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string returnableToolName = "Fishing Rod";
    [SerializeField] private GameObject toolPickupReference;

    public void Interact(GameObject interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return;

        if (Time.time - Inventory.lastToolInteractionTime < Inventory.toolInteractionCooldown) return;
        Inventory.lastToolInteractionTime = Time.time;

        for (int i = 0; i < inventory.toolSlots.Count; i++)
        {
            var item = inventory.toolSlots[i];
            if (item != null && item.itemName == returnableToolName)
            {
                inventory.toolSlots[i] = null;

                if (toolPickupReference != null)
                    toolPickupReference.SetActive(true);

                if (returnableToolName == "Fishing Rod")
                {
                    Debug.Log("🔕 Disabling fishing spots...");
                    foreach (FishingSpotEnabler spot in Object.FindObjectsByType<FishingSpotEnabler>(FindObjectsSortMode.None))
                    {
                        spot.SetFishingEnabled(false);
                    }
                }

                Debug.Log($"🔧 You returned the {returnableToolName}.");
                break;
            }
        }
    }
}
