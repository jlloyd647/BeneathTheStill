using UnityEngine;

public class Bobber : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float bobSpeed = 2f;
    public float bobHeight = 0.1f;

    [Header("Bite Timing")]
    public float minBiteDelay = 2f;
    public float maxBiteDelay = 5f;

    [Header("Audio")]
    public AudioClip biteSound;

    [Header("Fishing Data")]
    public FishTable fishTable;

    private Vector3 startPos;
    private float biteTimer;
    private bool biteTriggered = false;
    private AudioSource audioSource;

    void Start()
    {
        startPos = transform.position;
        biteTimer = Random.Range(minBiteDelay, maxBiteDelay);
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Gentle bob
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPos + new Vector3(0f, offset, 0f);

        // Bite countdown
        if (!biteTriggered)
        {
            biteTimer -= Time.deltaTime;
            if (biteTimer <= 0f)
            {
                TriggerBite();
            }
        }
    }

    void TriggerBite()
    {
        biteTriggered = true;
        Debug.Log("🎯 Bite!");

        if (biteSound != null)
        {
            audioSource.PlayOneShot(biteSound);
        }

        // Could add particle or bobber flash here
    }

    public void TryReelIn(GameObject interactor)
    {
        Debug.Log("🎣 TryReelIn called");

        if (biteTriggered && fishTable != null)
        {
            FishData caught = RollFish(fishTable);
            Debug.Log($"🎉 You caught a {caught.fishName} ({caught.rarity})!");

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory != null)
            {
                InventoryItem item = new InventoryItem
                {
                    itemName = caught.fishName,
                    icon = caught.icon, // Optional icon
                    itemType = ItemType.Fish,
                    quantity = 1,
                    stackable = true
                };

                bool success = inventory.AddItem(item);
                if (!success)
                    Debug.LogWarning("📦 Inventory full or item could not be added!");
            }
        }
        else
        {
            Debug.Log("❌ You reeled in too early.");
        }

        Destroy(gameObject); // Remove bobber no matter what
    }

    private FishData RollFish(FishTable table)
    {
        float roll = Random.value;
        float cumulative = 0f;

        foreach (var entry in table.fishEntries)
        {
            cumulative += entry.chance;
            if (roll <= cumulative)
            {
                return entry.fish;
            }
        }

        // Fallback
        return table.fishEntries.Length > 0 ? table.fishEntries[0].fish : null;
    }
}
