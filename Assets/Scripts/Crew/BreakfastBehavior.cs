using UnityEngine;
using System.Collections.Generic;

public class BreakfastBehavior : MonoBehaviour
{
    [Header("Breakfast Settings")]
    public BreakfastManager breakfastManager; // Set via Inspector
    public CrewMateSchedule schedule;

    [Tooltip("What in-game time this crew should go to breakfast (e.g., 7.25 = 7:15am)")]
    public float breakfastTime = 7.00f;

    public List<Transform> pathToPlateThenTable;
    public List<Transform> pathToPantryThenTable;

    private bool hasMovedToBreakfast = false;

    void Update()
    {
        if (hasMovedToBreakfast) return;

        if (TimeManager.Instance == null) return;

        float currentTime = TimeManager.Instance.timeOfDay;

        if (currentTime >= breakfastTime)
        {
            hasMovedToBreakfast = true;

            float eatUntil = currentTime + 1f;

            bool foodReady = breakfastManager != null && breakfastManager.playerMadeBreakfast;

            CrewMate crewMate = GetComponent<CrewMate>();
            if (crewMate != null)
            {
                if (foodReady)
                {
                    schedule.OverridePath(pathToPlateThenTable, eatUntil);
                    crewMate.stats.ChangeSpirit(+10);
                    Debug.Log($"{gameObject.name} heading to breakfast via 🍽️ plate path.");
                }
                else
                {
                    crewMate.stats.ChangeSpirit(-5);
                    schedule.OverridePath(pathToPantryThenTable, eatUntil);
                    Debug.Log($"{gameObject.name} heading to breakfast via 🥫 pantry path.");
                }
            }
        }
    }
}
