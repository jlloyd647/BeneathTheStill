using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Cooking/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public Sprite icon;
    // public List<Ingredient> requiredIngredients;
    public float cookingTime;
    public float choppingTime;
    // public AudioClip cookingSizzle;
    // public AudioClip choppingSound;
}