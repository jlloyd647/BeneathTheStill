using System.Collections.Generic;
using UnityEngine;

public class PrepJarInventory : MonoBehaviour
{
    private Dictionary<IngredientType, int> inventory = new Dictionary<IngredientType, int>();

    // Add chopped ingredient(s)
    public void AddIngredient(IngredientType type, int amount = 1)
    {
        if (inventory.ContainsKey(type))
            inventory[type] += amount;
        else
            inventory[type] = amount;
    }

    // Check how many of a type you have
    public int GetIngredientAmount(IngredientType type)
    {
        return inventory.TryGetValue(type, out int amount) ? amount : 0;
    }

    // Clear the inventory (for end-of-day reset)
    public void ClearAll()
    {
        inventory.Clear();
    }
}
