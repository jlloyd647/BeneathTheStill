using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;

    void Start()
    {
        currentStamina = maxStamina;
    }

    public void UseStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0, maxStamina);
        Debug.Log($"Stamina: {currentStamina}/{maxStamina}");
    }
}
