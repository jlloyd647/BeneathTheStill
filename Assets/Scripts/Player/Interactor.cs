using UnityEngine;
using System.Collections;

public class Interactor : MonoBehaviour
{
    public InteractBarController interactBarPrefab;
    private InteractBarController currentBar;

    public void StartInteraction(Transform target, float duration, bool allowMovement = true)
    {
        if (duration <= 0f || target == null) return;

        // Instantiate and show the bar above the given target
        Vector3 barPosition = target.position + Vector3.up * 1.5f;
        currentBar = Instantiate(interactBarPrefab, barPosition, Quaternion.identity);
        currentBar.Show(target, duration);

        // Only disable movement if this Interactor has a PlayerController
        if (!allowMovement && TryGetComponent<PlayerController>(out var player))
        {
            player.enabled = false;
        }

        StartCoroutine(EndInteraction(duration));
    }

    private IEnumerator EndInteraction(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentBar != null)
        {
            currentBar.Hide();
            currentBar = null;
        }

        // Re-enable movement if this is the player
        if (TryGetComponent<PlayerController>(out var player))
        {
            player.enabled = true;
        }
    }
}
