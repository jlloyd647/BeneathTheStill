using UnityEngine;

public class ItemBox : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "Item";
    [SerializeField] private int giveQuantity = 1;
    [SerializeField] private IngredientSubType ingredientSubType = IngredientSubType.None;

    public void Interact(GameObject interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();
        if (inventory == null) return;

        InventoryItem newItem = new InventoryItem
        {
            itemName = itemName,
            itemType = ItemType.Ingredient,
            ingredientSubType = ingredientSubType,
            quantity = giveQuantity,
            stackable = true
        };

        inventory.AddItem(newItem);
        Debug.Log($"📦 Took {giveQuantity} {itemName}(s) from the box.");
    }
}
