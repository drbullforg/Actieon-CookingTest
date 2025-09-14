using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Cooking/Inventory")]
[System.Serializable]
public class InventoryData : ScriptableObject
{
    public List<IngredientAmount> items = new List<IngredientAmount>();
}
