using UnityEngine;

public class CrewMate : MonoBehaviour, IInteractable
{
    [Header("Crew Identity")]
    public string crewName = "Crew Member";

    [TextArea]
    public string[] dialogueLines;

    [Header("Stats")]
    public CrewStats stats = new CrewStats();

    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    public void Talk()
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        string line = dialogueLines[Random.Range(0, dialogueLines.Length)];
        dialogueManager.ShowDialogue(crewName, line);
    }

    public void Interact(GameObject interactor)
    {
        Talk();
    }
}
