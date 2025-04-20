using UnityEngine;
using UnityEngine.UI;

public class StationProgressBar : MonoBehaviour
{
    [SerializeField] private Canvas barCanvas;
    [SerializeField] private Slider slider;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    [SerializeField] private bool alwaysFaceCamera = true;

    private Transform followTarget;

    void Awake()
    {
        if (barCanvas == null)
            barCanvas = GetComponentInChildren<Canvas>();

        if (barCanvas != null)
            barCanvas.enabled = false;
    }

    void Update()
    {
        if (followTarget != null)
            transform.position = followTarget.position + offset;

        if (alwaysFaceCamera && Camera.main != null)
            barCanvas.transform.LookAt(Camera.main.transform);
    }

    public void SetProgress(float progress)
    {
        slider.value = Mathf.Clamp01(progress);
    }

    public void Show()
    {
        if (barCanvas != null)
            barCanvas.enabled = true;
    }

    public void Hide()
    {
        if (barCanvas != null)
            barCanvas.enabled = false;
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
}
