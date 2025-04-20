using UnityEngine;

public class RiggingPoint : MonoBehaviour, IInteractable
{
    public Color rigColor;
    [Range(0, 100)]
    public float tightness = 0f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = rigColor;
    }

    // Lazy Loaded
    public void SetColor(Color color)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        rigColor = color;
        spriteRenderer.color = color;
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"🪢 Interacted with {name} via player");

        if (RiggingManager.Instance == null)
        {
            Debug.LogWarning("⚠️ RiggingManager.Instance is null!");
            return;
        }

        RiggingManager.Instance.OnRiggingSelected(this);
    }

    public void Tighten(float amount)
    {
        tightness = Mathf.Clamp(tightness + amount, 0, 100);
    }

    public void Loosen(float amount)
    {
        tightness = Mathf.Clamp(tightness - amount, 0, 100);
    }

    public bool IsFullyTight() => tightness >= 100f;

    // 🆕 Optional interaction duration and movement flag
    public float GetInteractDuration() => 1.5f; // seconds
    public bool AllowMovementDuringInteract() => false;
}
