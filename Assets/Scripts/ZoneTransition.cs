using UnityEngine;

public class ZoneTransition : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private GameObject[] zonesToEnable;
    [SerializeField] private GameObject[] zonesToDisable;
    [SerializeField] private bool autoTrigger = false;

    public void Interact(GameObject interactor)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = targetPosition.position;
            Camera.main.GetComponent<CameraFollow>().SnapToTarget();

            foreach (var go in zonesToEnable)
                if (go != null) go.SetActive(true);

            foreach (var go in zonesToDisable)
                if (go != null) go.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    if (!autoTrigger) return;

    if (other.CompareTag("Player"))
    {
        Debug.Log("<color=yellow>Auto-transition triggered</color>");
        Interact(other.gameObject);
    }
}
}
