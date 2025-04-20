using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Day Settings")]
    public float timeOfDay = 6f; // Start at 6 AM
    public float dayLengthInSeconds = 1440f; // One full day = 24 minutes

    public int hours => Mathf.FloorToInt(timeOfDay);
    public int minutes => Mathf.FloorToInt((timeOfDay - hours) * 60f);
    
    [Header("UI")]
    public TextMeshProUGUI timeDisplay;

    private static TimeManager instance;
    public static TimeManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Kill the duplicate
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        float timePerDayUnit = 24f / dayLengthInSeconds;
        timeOfDay += Time.deltaTime * timePerDayUnit;

        if (timeOfDay >= 24f)
        {
            timeOfDay = 0f;
            Debug.Log("🌅 A new day begins!");
        }

        if (timeDisplay != null)
        {
            timeDisplay.text = $"{hours:00}:{minutes:00}";
        }
    }

    public void AdvanceTime(float hoursToAdvance)
    {
        timeOfDay += hoursToAdvance;

        if (timeOfDay >= 24f)
        {
            timeOfDay -= 24f;
            Debug.Log("🕛 Passed midnight. New day continues.");
        }

        Debug.Log($"🛏️ You slept and woke up at {hours:00}:{minutes:00}");
    }
}
