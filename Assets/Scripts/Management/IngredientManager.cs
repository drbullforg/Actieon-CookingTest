using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager instance;

    public RecipeData recipeData;

    [Header("Ingredient Viewer")]
    public Transform ingredientContent;
    public GameObject prefabIngredientItem;
    public List<IngredientItem> ingredientItems = new List<IngredientItem>();

    private void Awake() {
        instance = this;
    }
    public void ShowIngredientInfo(RecipeData _recipeData) {
        ClearIngredients();
        recipeData = _recipeData;

        foreach(IngredientAmount item in recipeData.ingredientRequirement) {
            GameObject ingredient = Instantiate(prefabIngredientItem, ingredientContent);
            IngredientItem ingredientItem = ingredient.GetComponent<IngredientItem>();
            ingredientItem.SetItem(item.ingredient, item.amount);
            ingredientItems.Add(ingredientItem);
        }
    }

    public void ClearIngredients() {
        int count = ingredientContent.childCount;
        for (int i = 0; i < count; i++) {
            DestroyImmediate(ingredientContent.GetChild(0).gameObject);
        }

        ingredientItems.Clear();
    }

    public void UpdateAmount() {
        foreach(var item in ingredientItems) {
            item.UpdateAmount();
        }
    }
}
