using UnityEngine;
using UnityEngine.UI;

public class InteractBarController : MonoBehaviour
{
    public Slider interactSlider;
    public GameObject barRoot;

    private Transform target;
    private float interactTime;
    private float elapsedTime;
    private bool isActive;
    private System.Action onCompleteCallback;

    [Header("Options")]
    public bool destroyOnFinish = false;
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    [Header("Advanced")]
    public bool useManualControl = false; // <--- New flag

    public void Show(Transform followTarget, float duration, System.Action onComplete = null)
    {
        if (duration <= 0f || followTarget == null) return;

        target = followTarget;
        interactTime = duration;
        elapsedTime = 0f;
        isActive = true;
        onCompleteCallback = onComplete;

        barRoot?.SetActive(true);
    }

    public void Hide()
    {
        isActive = false;
        barRoot?.SetActive(false);
        onCompleteCallback = null;

        if (destroyOnFinish)
            Destroy(gameObject);
    }

    public void SetProgress(float value)
    {
        if (interactSlider != null)
            interactSlider.value = Mathf.Clamp01(value);
    }

    public void SetManualProgress(float normalizedProgress)
    {
        if (interactSlider != null)
            interactSlider.value = Mathf.Clamp01(normalizedProgress);
    }

    void Update()
    {
        if (!isActive || target == null) return;

        transform.position = target.position + offset;

        if (!useManualControl)
        {
            elapsedTime += Time.deltaTime;
            interactSlider.value = Mathf.Clamp01(elapsedTime / interactTime);

            if (elapsedTime >= interactTime)
            {
                isActive = false;
                barRoot?.SetActive(false);
                onCompleteCallback?.Invoke();

                if (destroyOnFinish)
                    Destroy(gameObject);
            }
        }
    }
}