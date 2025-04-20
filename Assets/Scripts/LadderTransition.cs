using UnityEngine;

public class LadderTransition : MonoBehaviour, IInteractable
{
    [SerializeField] private string targetSceneName = "TopDeckScene";
    [SerializeField] private string targetSpawnPointName = "Spawn_LadderTop";

  public void Interact(GameObject interactor)
  {
      SpawnManager.NextSpawnPointName = targetSpawnPointName;
      SceneTransitionManager.Instance.TransitionToScene(targetSceneName);
  }
}