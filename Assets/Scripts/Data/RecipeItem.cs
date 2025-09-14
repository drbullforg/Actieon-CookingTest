using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItem : MonoBehaviour
{
    public RecipeData recipeData;

    Toggle toggle;

    public Image thumbnail;
    public TextMeshProUGUI nameText;

    private void Awake() {
        toggle = GetComponent<Toggle>();
    }
    public void SetItem(RecipeData _recipeData) {
        recipeData = _recipeData;

        thumbnail.sprite = recipeData.recipeIcon;
        thumbnail.SetNativeSize();

        nameText.text = recipeData.recipeName;
    }

    public void Selected() {
        if (toggle.isOn) {
            IngredientManager.instance.ShowIngredientInfo(recipeData);
            GameSystemManager.instance.CheckCook(recipeData);
        } else {
            GameSystemManager.instance.ClearRecipe();
        }
    }
}
