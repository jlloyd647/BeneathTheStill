using TMPro;
using UnityEngine;

public class CrewDialogBubble : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float lifetime = 3f;

    private float timer;

    public void Show(string message, float duration = 3f)
    {
        text.text = message;
        timer = duration;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        // Optional: make it face the camera
        transform.forward = Camera.main.transform.forward;
    }
}
