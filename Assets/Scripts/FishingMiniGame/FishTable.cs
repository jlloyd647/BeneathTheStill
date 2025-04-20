using UnityEngine;

[CreateAssetMenu(fileName = "New Fish Table", menuName = "Fishing/Fish Table")]
public class FishTable : ScriptableObject
{
    public FishEntry[] fishEntries;
}

[System.Serializable]
public class FishEntry
{
    public FishData fish;
    [Range(0f, 1f)] public float chance; // 0.0 - 1.0
}