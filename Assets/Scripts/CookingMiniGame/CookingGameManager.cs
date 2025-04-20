using UnityEngine;

public class CookingGameManager : MonoBehaviour
{
    public static CookingGameManager Instance { get; private set; }

    public bool IsCooking { get; private set; } = false;
    public InventoryItem CurrentCookItem { get; private set; }
    public int RequiredFlips { get; private set; } = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void BeginCookingItem(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("⚠️ Tried to start cooking with a null item.");
            return;
        }

        CurrentCookItem = item;
        RequiredFlips = 4; // You can make this dynamic per item later
        IsCooking = true;

        Debug.Log($"🍳 Cooking started: {item.itemName} | Flips needed: {RequiredFlips}");
    }

    public void StopCooking()
    {
        Debug.Log("🥄 Cooking has ended.");
        IsCooking = false;
        CurrentCookItem = null;
        RequiredFlips = 4;
    }
}
