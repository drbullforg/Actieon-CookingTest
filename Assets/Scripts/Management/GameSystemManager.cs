using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CookState
{
    Standby = 0,
    Cooking = 1,
    Finished = 2
}
public enum CookAnimState
{
    Idle = 0,
    StartCooking = 1,
    Cooking = 2,
    Success = 4,
    Failed = 5
}
public class GameSystemManager : MonoBehaviour
{
    public static GameSystemManager instance;
    
    public CookState state;

    [Header("Data Table")]
    public RecipeTable recipeTable;
    public IngredientTable ingredientTable;

    [Header("Cooking Data")]
    public RecipeData currentRecipe;
    public string cookingRecipe;
    public Button btnStartCook;
    public CookingTimer timer;
    public AnimationController animPot;
    public int successRate = 80;

    [Header("Recipe Data")]
    public int filter = -1;
    public TMP_InputField searchText;
    public GameObject panelFilter;
    public Toggle[] btnFilters;

    [Header("Result Data")]
    public ResultManager resultSuccess;
    public ResultManager resultFailed;

    [Header("Shop Data")]
    public QuickShopManager quickShopManager;
    private void Awake() {
        instance = this;

        if (PlayerPrefs.HasKey("CookingState")) {
            SetCookState((CookState)PlayerPrefs.GetInt("CookingState"));
            if (state == CookState.Cooking) {
                SetAnimation(CookAnimState.Cooking);
            }

            if (PlayerPrefs.HasKey("CookingRecipe")) {
                cookingRecipe = PlayerPrefs.GetString("CookingRecipe");
            }
        } else {
            SetCookState(CookState.Standby);
        }
    }

    private void Start() {
        RecipeManager.instance.InitRecipe();
    }

    public void CheckCook(RecipeData recipeData) {
        currentRecipe = recipeData;
        SetTimer(recipeData.cookingTime);

        if (RecipeManager.instance.CanCook(recipeData) && PlayerManager.instance.HasEnergy(recipeData.energyCost)) {
            btnStartCook.interactable = state == CookState.Standby;
        } else {
            btnStartCook.interactable = false;
        }
    }

    public void CookNow() {
        foreach(var item in currentRecipe.ingredientRequirement) {
            PlayerManager.instance.UseItem(item.ingredient.ingredientName, item.amount);
        }
        IngredientManager.instance.UpdateAmount();
        PlayerManager.instance.UseEnergy(currentRecipe.energyCost);

        SetCookState(CookState.Cooking);
        SetAnimation(CookAnimState.StartCooking);
        btnStartCook.interactable = false;

        //StartCoroutine(DelayCooking());
        timer.StartCooking();
        cookingRecipe = currentRecipe.recipeName;
    }

    IEnumerator DelayCooking() {
        yield return new WaitForSeconds(4);
        timer.StartCooking();
    }

    public void CookFinished() {
        int rand = Random.Range(0, 100);
        bool isSuccess = true;
        if(rand <= successRate) {
            SetAnimation(CookAnimState.Success);
        } else {
            SetAnimation(CookAnimState.Failed);
            isSuccess = false;
        }
        SetCookState(CookState.Finished);

        StartCoroutine(DelayResult(isSuccess));
    }

    IEnumerator DelayResult(bool isSuccess) {
        yield return new WaitForSeconds(2);

        if (isSuccess) {
            resultSuccess.SetItem(RecipeManager.instance.GetRecipeFromName(cookingRecipe).recipeData);
        } else {
            resultFailed.SetItem(RecipeManager.instance.GetRecipeFromName(cookingRecipe).recipeData);
        }
    }

    public void ResultFinished() {
        cookingRecipe = "";
        PlayerPrefs.DeleteKey("CookingRecipe");
        PlayerPrefs.DeleteKey("CookingEndTime");

        SetAnimation(CookAnimState.Idle);
        SetCookState(CookState.Standby);
    }

    public void SetTimer(int _time) {
        timer.cookingDuration = _time;
        timer.timer.text = FormatTime(_time);
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

    public void SetFilter() {
        filter = -1;
        for(int i = 0; i < btnFilters.Length; i++) {
            if (btnFilters[i].isOn) {
                filter = i;
                break;
            }
        }
        Searching();
    }

    public void Searching() {
        RecipeManager.instance.SetPage(RecipeManager.instance.GetListRecipe(filter, searchText.text));
    }

    public void SetCookState(CookState cookState) {
        state = cookState;
    }

    public void SetAnimation(CookAnimState cookAnimState) {
        animPot.PlayAnimation((int)cookAnimState);
    }

    public void GetQuickShop(IngredientData ingredient) {
        quickShopManager.SetItem(ingredient);
    }

    private void OnDisable() {
        if (state == CookState.Finished) state = CookState.Standby;
        PlayerPrefs.SetInt("CookingState", (int)state);
        if(cookingRecipe != null && cookingRecipe != "") {
            PlayerPrefs.SetString("CookingRecipe", cookingRecipe);
        }
    }
}
