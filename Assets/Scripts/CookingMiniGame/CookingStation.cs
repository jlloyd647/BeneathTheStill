using UnityEngine;

public class CookingStation : MonoBehaviour
{
    [Header("Flip Settings")]
    [SerializeField] private float timeToRed = 10f;

    private SpriteRenderer spriteRenderer;
    private bool playerInRange = false;

    private float flipTimer = 0f;
    private int flipCount = 0;
    private int requiredFlips = 4;
    private bool isRed = false;
    private float cookScore = 0f;

    private Inventory playerInventory;
    private Color coldColor = Color.blue;
    private Color hotColor = Color.red;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = coldColor;
    }

    void Update()
    {
        if (CookingGameManager.Instance == null || !CookingGameManager.Instance.IsCooking) return;
        
        if (!CookingGameManager.Instance.IsCooking) return;

        flipTimer += Time.deltaTime;

        float t = flipTimer / timeToRed;
        spriteRenderer.color = Color.Lerp(coldColor, hotColor, Mathf.Clamp01(t));

        if (!isRed && t >= 1f)
        {
            isRed = true;
            Debug.Log("🔥 Pan is ready to flip.");
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HandleFlip();
        }
    }

    private void HandleFlip()
    {
        if (!CookingGameManager.Instance.IsCooking) return;

        flipCount++;

        float heat = flipTimer / timeToRed;
        float flipScore;

        if (heat < 1f)
        {
            // Early flip: score from 0–100
            flipScore = heat * 100f;
        }
        else
        {
            // Late flip: score from 100–150
            float overcookTime = heat - 1f;
            flipScore = 100f + (overcookTime * 50f);
        }

        flipScore = Mathf.Clamp(flipScore, 0f, 150f);
        cookScore += flipScore;

        Debug.Log($"🔄 Flip {flipCount}/{requiredFlips} | Heat: {heat:F2} | Flip Score: {flipScore:F1} | Total: {cookScore:F1}");

        flipTimer = 0f;
        isRed = false;
        spriteRenderer.color = Color.blue;

        if (flipCount >= requiredFlips)
        {
            FinishCooking();
        }
    }

    private void FinishCooking()
    {
        Debug.Log($"✅ Cooking complete! Total score: {cookScore:F1}");

        InventoryItem raw = CookingGameManager.Instance.CurrentCookItem;

        string quality;
        if (cookScore < 300f) quality = "Undercooked";
        else if (cookScore < 375f) quality = "Good";
        else if (cookScore < 425f) quality = "Perfect";
        else if (cookScore < 500f) quality = "Crispy";
        else quality = "Burnt";

        string cookedName = $"{quality} {raw.itemName}";

        InventoryItem cookedItem = new InventoryItem
        {
            itemName = cookedName,
            itemType = raw.itemType,
            ingredientSubType = IngredientSubType.Cooked,
            quantity = 1,
            stackable = true
        };

        playerInventory.AddItem(cookedItem);
        playerInventory.RemoveItem(raw.itemName, 1);

        Debug.Log($"🎖️ Result: {cookedName} ({cookScore:F0} points)");

        CookingGameManager.Instance.StopCooking();
        ResetStation();
    }

    private void ResetStation()
    {
        flipCount = 0;
        cookScore = 0f;
        flipTimer = 0f;
        isRed = false;
        spriteRenderer.color = coldColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<Inventory>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }
}
