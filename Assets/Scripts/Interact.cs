using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private InteractBarController interactBarPrefab;

    private InteractBarController currentBar;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        foreach (var hit in hits)
        {
            var interactables = hit.GetComponents<IInteractable>();

            foreach (var interactable in interactables)
            {
                float duration = interactable.GetInteractDuration();
                bool canMove = interactable.AllowMovementDuringInteract();

                if (duration <= 0f)
                {
                    interactable.Interact(gameObject);
                    return;
                }

                // Freeze movement if needed
                if (!canMove)
                    GetComponent<PlayerController>()?.Freeze();

                // Instantiate and show interact bar
                currentBar = Instantiate(interactBarPrefab);
                currentBar.Show(transform, duration, () =>
                {
                    // Call interact when the bar finishes
                    interactable.Interact(gameObject);

                    if (!canMove)
                        GetComponent<PlayerController>()?.Unfreeze();
                });

                return;
            }
        }

        Debug.Log("🧐 Nothing interactable nearby.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
