using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    public static GameSystemManager instance;

    [Header("Data Table")]
    public RecipeTable recipeTable;
    public IngredientTable ingredientTable;

    [Header("Cooking Data")]
    public RecipeData currentRecipe;
    public Button btnStartCook;
    public TextMeshProUGUI timer;
    public AnimationController animPot;

    [Header("Recipe Data")]
    public int filter = -1;
    public TMP_InputField searchText;
    public GameObject panelFilter;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        RecipeManager.instance.InitRecipe();
    }

    public void CheckCook(RecipeData recipeData) {
        currentRecipe = recipeData;
        SetTimer(recipeData.cookingTime);

        if (RecipeManager.instance.CanCook(recipeData)) {
            btnStartCook.interactable = true;
        } else {
            btnStartCook.interactable = false;
        }
    }

    public void CookNow() {
        foreach(var item in currentRecipe.ingredientRequirement) {
            PlayerManager.instance.UseItem(item.ingredient.ingredientName, item.amount);
        }
        IngredientManager.instance.UpdateAmount();
    }

    public void SetTimer(int _time) {
        timer.text = FormatTime(_time);
    }

    public static string FormatTime(int totalSeconds) {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

    public void ClearRecipe() {
        IngredientManager.instance.ClearIngredients();
        btnStartCook.interactable = false;
    }

    public void ToggleFilterPanel() {
        panelFilter.SetActive(!panelFilter.activeSelf);
    }

    public void SetFilter(int _filter) {
        filter = _filter;
        Searching();
    }

    public void Searching() {
        RecipeManager.instance.SetPage(RecipeManager.instance.GetListRecipe(filter, searchText.text));
    }
}
