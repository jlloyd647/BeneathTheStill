using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI spiritText;

    public float autoHideTime = 4f;

    private float hideTimer = 0f;

    void Update()
    {
        if (dialogueBox.activeSelf)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
            {
                dialogueBox.SetActive(false);
            }
        }
    }

    public void ShowDialogue(string speaker, string line, int? spirit = null)
    {
        dialogueBox.SetActive(true);
        speakerNameText.text = speaker;
        dialogueText.text = line;

        if (spirit.HasValue)
        {
            spiritText.text = $"Spirit: {spirit.Value}";
            spiritText.gameObject.SetActive(true);
        }
        else
        {
            spiritText.gameObject.SetActive(false);
        }

        hideTimer = autoHideTime;
    }
}
