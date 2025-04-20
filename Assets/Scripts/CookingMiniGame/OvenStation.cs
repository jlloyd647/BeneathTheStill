using UnityEngine;

public class OvenStation : MonoBehaviour, IInteractable
{
    public Transform mealSpawnPoint;
    public GameObject mealPrefab;
    public float cookTime = 5f;

    private bool isCooking = false;
    private float cookTimer = 0f;
    private bool mealReady = false;
    public BreakfastManager breakfastManager;

    public InteractBarController interactBarPrefab; // Drag your prefab here in Inspector
    private InteractBarController activeBar;

    GameObject wrenObject;

    void Awake()
    {
        wrenObject = GameObject.Find("Wren");
        if (wrenObject == null)
        {
            Debug.LogWarning("<color=red>⚠️ Wren object not found in scene!</color>");
        }
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log("<color=cyan>🔎 Interacted with OvenStation</color>");

        Inventory inventory = interactor.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("<color=red>🚫 Interactor has no Inventory component!</color>");
            return;
        }

        if (!isCooking && !mealReady)
        {
            var cooked = inventory.FindIngredientBySubType(IngredientSubType.Cooked);
            var choppedPotato = inventory.FindItemByName("Chopped Potato");

            Debug.Log($"<color=yellow>🔍 Looking for ingredients: Cooked={(cooked != null)}, Chopped Potato={(choppedPotato != null)}</color>");

            if (cooked != null && choppedPotato != null)
            {
                inventory.RemoveItem(cooked.itemName);
                inventory.RemoveItem(choppedPotato.itemName);

                isCooking = true;
                cookTimer = cookTime;

                // Instantiate the interact bar above this oven station
                if (interactBarPrefab != null)
                {
                    activeBar = Instantiate(interactBarPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);
                    activeBar.Show(transform, cookTime, () =>
                    {
                        isCooking = false;
                        mealReady = true;
                        Debug.Log("<color=lime>✅ Cooking complete! Ready to pull out the meal.</color>");
                    });
                }
                else
                {
                    Debug.LogWarning("InteractBarPrefab not assigned on OvenStation.");
                }
            }
            else
            {
                Debug.Log("<color=red>⚠️ Missing required ingredients! Need both Cooked and Chopped Potato.</color>");
            }
        }
        else if (mealReady)
        {
            Instantiate(mealPrefab, mealSpawnPoint.position, Quaternion.identity);
            mealReady = false;
            TriggerCrewDialog("Wren", "Something smells good!", 3f);
            breakfastManager.playerMadeBreakfast = true;
            Debug.Log("<color=magenta>🍽️ Meal is ready and has been spawned!</color>");
        }
        else
        {
            Debug.Log("<color=orange>⏳ Still cooking...</color>");
        }
    }

    private void Update()
    {
        if (isCooking && activeBar == null)
        {
            cookTimer -= Time.deltaTime;
            if (cookTimer <= 0f)
            {
                isCooking = false;
                mealReady = true;
                Debug.Log("<color=lime>✅ Cooking complete! Ready to pull out the meal.</color>");
            }
        }
    }

    private void TriggerCrewDialog(string crewName, string message, float duration = 3f)
    {
        GameObject crewObject = GameObject.Find(crewName);

        if (crewObject != null)
        {
            CrewDialogController dialog = crewObject.GetComponent<CrewDialogController>();
            if (dialog != null)
            {
                dialog.Say(message, duration);
            }
            else
            {
                Debug.LogWarning($"CrewDialogController not found on {crewName}.");
            }
        }
        else
        {
            Debug.LogWarning($"Crew member '{crewName}' not found in scene.");
        }
    }
}
