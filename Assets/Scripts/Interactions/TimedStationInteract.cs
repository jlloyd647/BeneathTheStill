using UnityEngine;
using System;

public class TimedStationInteraction : MonoBehaviour
{
    [Header("Progress Settings")]
    public float totalTime = 3f;
    public InteractBarController interactBarPrefab;

    private float elapsedTime = 0f;
    private bool isRunning = false;
    private bool isPlayerInRange = false;
    private GameObject currentPlayer;
    private Action onCompleteCallback;
    private InteractBarController activeBar;

    private void Update()
    {
        if (isRunning && isPlayerInRange)
        {
            elapsedTime += Time.deltaTime;

            if (activeBar != null)
                activeBar.SetManualProgress(elapsedTime / totalTime);

            if (elapsedTime >= totalTime)
            {
                FinishInteraction();
            }
        }
    }

    public void StartInteraction(GameObject player, Action onComplete, float overrideTime = -1f)
    {
        currentPlayer = player;
        onCompleteCallback = onComplete;
        totalTime = overrideTime > 0f ? overrideTime : totalTime;

        if (elapsedTime >= totalTime)
            elapsedTime = 0f;

        isRunning = true;

        if (interactBarPrefab != null && activeBar == null)
        {
            activeBar = Instantiate(interactBarPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity, transform);
            activeBar.destroyOnFinish = false;
            activeBar.useManualControl = true; // <-- Tell the bar to stay passive
        }

        if (isPlayerInRange)
        {
            activeBar?.SetManualProgress(elapsedTime / totalTime); // Update progress before show
            activeBar?.Show(transform, totalTime);
        }
    }

    public void SetPlayerInRange(bool inRange)
    {
        isPlayerInRange = inRange;

        if (!isRunning || activeBar == null) return;

        if (inRange)
        {
            activeBar?.SetManualProgress(elapsedTime / totalTime);
            activeBar?.Show(transform, totalTime);
        }
        else
        {
            activeBar?.Hide();
        }
    }

    private void FinishInteraction()
    {
        isRunning = false;
        activeBar?.Hide();
        onCompleteCallback?.Invoke();
        elapsedTime = 0f;
    }
}