using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredientTable", menuName = "Cooking/IngredientTable")]
[System.Serializable]
public class IngredientTable: ScriptableObject
{
    public List<IngredientData> table = new List<IngredientData>();
}
