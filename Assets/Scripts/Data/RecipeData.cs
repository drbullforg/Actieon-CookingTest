using System.Collections.Generic;
using UnityEngine;

public enum RecipeRarity
{
    Common = 0,
    Rare = 1,
    Epic = 2
}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Cooking/Recipe")]
[System.Serializable]
public class RecipeData: ScriptableObject
{
    public string recipeName;
    public Sprite recipeIcon;
    public RecipeRarity recipeRarity;
    public int cookingTime;
    public List<IngredientAmount> ingredientRequirement;
}


[System.Serializable]
public class IngredientAmount
{
    public IngredientData ingredient;
    public int amount;
}