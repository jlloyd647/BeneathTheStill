using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ScheduleEntry
{
    public float timeOfDay; // e.g., 6.5 for 6:30 AM
    public List<Transform> path;
}

public class CrewMateSchedule : MonoBehaviour
{
    public List<ScheduleEntry> dailySchedule;

    private List<Transform> currentPath = new();
    private int currentPathIndex = 0;
    private float speed = 1.5f;

    private TimeManager timeManager;

    private bool hasOverridePath = false;
    private List<Transform> overridePath;
    private float overrideUntilTime = -1f;

    private float blockScheduleUntil = -1f; // 🧱 prevents default path from kicking in

    private bool isLingering = false;
    private float lingerEndTime = -1f;

    void Start()
    {
        timeManager = FindAnyObjectByType<TimeManager>();
        UpdatePath();
    }

    void Update()
    {
        float currentTime = timeManager.timeOfDay;

        // Check if override has expired
        if (hasOverridePath && currentTime >= overrideUntilTime)
        {
            hasOverridePath = false;
            overridePath = null;
        }

        UpdatePath();
        FollowPath();
    }

    void UpdatePath()
    {
        float currentTime = timeManager.timeOfDay;

        // 🛡️ Suppress default schedule while blocking is active
        if (currentTime < blockScheduleUntil)
        {
            return;
        }

        // If override is active, keep using it
        if (hasOverridePath)
        {
            if (overridePath != currentPath)
            {
                currentPath = overridePath;
                currentPathIndex = 0;
            }
            return;
        }

        // Otherwise, proceed with regular schedule
        ScheduleEntry latestEntry = null;

        foreach (var entry in dailySchedule)
        {
            if (currentTime >= entry.timeOfDay)
                latestEntry = entry;
        }

        if (latestEntry != null && latestEntry.path != currentPath)
        {
            currentPath = latestEntry.path;
            currentPathIndex = 0;
        }
    }

    void FollowPath()
    {
        // Debug.Log($"{gameObject.name} walking to: {currentPath[currentPathIndex].name}");
        if (isLingering)
        {
            if (timeManager.timeOfDay >= lingerEndTime) // Or use TimeManager.timeOfDay
            {
                isLingering = false;
            }
            else
            {
                return; // skip movement while lingering
            }
        }
        
        if (currentPath == null || currentPath.Count == 0) return;

        Transform target = currentPath[currentPathIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (isLingering)
            {
                if (timeManager.timeOfDay >= lingerEndTime)
                {
                    isLingering = false;
                    Debug.Log($"{gameObject.name} finished lingering at {target.name}. Advancing.");
                    currentPathIndex++;
                }
                return; // Wait until lingering ends
            }

            LingerPoint lingerPoint = target.GetComponent<LingerPoint>();
            if (lingerPoint != null && lingerEndTime < 0f)
            {
                isLingering = true;
                lingerEndTime = timeManager.timeOfDay + (lingerPoint.lingerDuration / 60f);
                Debug.Log($"{gameObject.name} is lingering at {target.name} for {lingerPoint.lingerDuration} in-game minutes (ends at {lingerEndTime:F2})");
                return; // Start lingering and wait
            }

            currentPathIndex++;
            if (currentPathIndex >= currentPath.Count)
                currentPathIndex = currentPath.Count - 1;
        }
    }

    public void OverridePath(List<Transform> newPath, float untilTime)
    {
        hasOverridePath = true;
        overridePath = newPath;
        overrideUntilTime = untilTime;

        blockScheduleUntil = untilTime; // ⛔ hold off daily pathing until override expires
        currentPathIndex = 0;

        Debug.Log($"{gameObject.name} received override path with {newPath.Count} waypoints. First: {newPath[0].name}");

        currentPath = newPath; // 🔥 force it immediately
    }
}
