using UnityEngine;
[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public ItemType itemType;
    public IngredientSubType ingredientSubType;
    public int quantity;
    public bool stackable;

    // Cooking-specific
    public int requiredFlips = 0;
    public bool isCooked = false;
    public bool isChopped = false;
    public Sprite icon;
}