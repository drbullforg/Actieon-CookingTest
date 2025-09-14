using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Cooking/Ingredient")]
[System.Serializable]
public class IngredientData: ScriptableObject
{
    public string ingredientName;
    public Sprite ingredientIcon;
    public int ingredientCost;
}