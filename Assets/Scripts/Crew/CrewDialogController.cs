using UnityEngine;

public class CrewDialogController : MonoBehaviour
{
    public GameObject dialogBubblePrefab;
    public Transform bubbleAnchor;

    private CrewDialogBubble bubbleInstance;

    void Start()
    {
        GameObject bubble = Instantiate(dialogBubblePrefab, bubbleAnchor.position, Quaternion.identity);
        bubble.transform.SetParent(bubbleAnchor, worldPositionStays: true);

        bubbleInstance = bubble.GetComponent<CrewDialogBubble>();
        bubble.SetActive(false);
    }

    public void Say(string message, float duration = 3f)
    {
        bubbleInstance.Show(message, duration);
    }
}