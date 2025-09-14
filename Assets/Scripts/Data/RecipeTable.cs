using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipeTable", menuName = "Cooking/RecipeTable")]
[System.Serializable]
public class RecipeTable: ScriptableObject
{
    public List<RecipeData> table = new List<RecipeData>();
}
