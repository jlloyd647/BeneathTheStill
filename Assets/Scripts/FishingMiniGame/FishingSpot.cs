using UnityEngine;

public class FishingSpot : MonoBehaviour, IInteractable
{
    public GameObject bobberPrefab;
    public float castDistance = 2f;
    public FishTable fishTable;

    private GameObject currentBobber;

    public void Interact(GameObject interactor)
    {
        PlayerController playerController = interactor.GetComponent<PlayerController>();
        Inventory inventory = interactor.GetComponent<Inventory>();

        if (inventory == null) return;

        // Check if the fishing rod is equipped
        bool hasFishingRod = false;
        foreach (var item in inventory.toolSlots)
        {
            if (item != null && item.itemName == "Fishing Rod")
            {
                hasFishingRod = true;
                break;
            }
        }

        if (!hasFishingRod)
        {
            Debug.Log("🚫 You need to be holding a fishing rod to fish.");
            return;
        }

        // If already fishing, try to reel in
        if (currentBobber != null)
        {
            Bobber bobberScript = currentBobber.GetComponent<Bobber>();
            if (bobberScript != null)
            {
                bobberScript.TryReelIn(interactor); // Pass player in for inventory access
            }

            currentBobber = null;

            if (playerController != null)
                playerController.ResetSpeed();

            return;
        }

        // Cast a new line
        Vector2 castDirection = interactor.transform.right;
        Vector3 spawnPos = interactor.transform.position + (Vector3)(castDirection.normalized * castDistance);
        currentBobber = Instantiate(bobberPrefab, spawnPos, Quaternion.identity);
        Debug.Log("✅ Bobber instantiated at: " + spawnPos);

        // Assign fish table to bobber
        Bobber newBobberScript = currentBobber.GetComponent<Bobber>();
        if (newBobberScript != null)
        {
            newBobberScript.fishTable = fishTable;
        }

        if (playerController != null)
            playerController.SetSpeedModifier(0f);

        Debug.Log("🎣 Cast line...");
    }
}
