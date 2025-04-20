using UnityEngine;

[System.Serializable]
public class FishData
{
    public string fishName;
    public Sprite icon;
    public Rarity rarity;
    public int value;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
