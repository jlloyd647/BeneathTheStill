using UnityEngine;

public class StartCooking : MonoBehaviour
{
    private bool playerInRange = false;
    private Inventory playerInventory;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<Inventory>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            InventoryItem proteinItem = playerInventory?.FindIngredientBySubType(IngredientSubType.Protein);
            if (proteinItem != null)
            {
                CookingGameManager.Instance.BeginCookingItem(proteinItem);
            }
            else
            {
                Debug.Log("❌ You need a Protein to start cooking.");
            }
        }
    }
}
