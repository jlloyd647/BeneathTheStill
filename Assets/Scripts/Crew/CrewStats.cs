using UnityEngine;

[System.Serializable]
public class CrewStats
{
    [Range(1, 100)]
    public int spirit = 50;

    [Range(1, 100)]
    public int superstition = 10;

    [Range(0f, 1f)]
    public float bond = 0.0f;

    public void ChangeSpirit(int delta)
    {
        spirit = Mathf.Clamp(spirit + delta, 1, 100);
    }

    public void ChangeSuperstition(int delta)
    {
        superstition = Mathf.Clamp(superstition + delta, 1, 100);
    }

    public void ChangeBond(float delta)
    {
        bond = Mathf.Clamp01(bond + delta);
    }
}
