using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject interactor);

    // Optional: Override if interaction takes time
    public virtual float GetInteractDuration() => 0f;

    // Optional: Override if player can move while interacting
    public virtual bool AllowMovementDuringInteract() => true;
}