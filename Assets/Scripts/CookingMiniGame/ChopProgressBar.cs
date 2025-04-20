using UnityEngine;
using UnityEngine.UI;
using System;

public class ChopProgressBar : MonoBehaviour
{
    public float chopTime = 3f;
    private float timer = 0f;
    private Slider slider;

    private Action onChopComplete;
    private bool isInitialized = false;

    public void Initialize(float time, Action callback)
    {
        Debug.Log("🔄 Initialize() called");

        gameObject.SetActive(true); // ensure it's visible & updatable

        chopTime = time;
        onChopComplete = callback;
        isInitialized = true;
        timer = 0f;

        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>(true);
            if (slider == null)
            {
                Debug.LogError("❌ Slider not found in children!");
            }
        }

        if (slider != null)
        {
            slider.maxValue = chopTime;
            slider.value = 0;
        }

        Debug.Log("✅ ChopProgressBar initialized with time: " + chopTime);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (slider != null)
        {
            slider.value = timer;
        }

        if (timer >= chopTime)
        {
            Debug.Log("🎉 Timer complete! Triggering callback.");
            isInitialized = false;

            onChopComplete?.Invoke(); // <== the line that fires your actual logic
            gameObject.SetActive(false);
        }
    }
}
