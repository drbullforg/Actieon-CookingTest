using DanielLochner.Assets.SimpleScrollSnap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager: MonoBehaviour
{
    public static RecipeManager instance;

    public PlayerManager playerInventory;

    [Header("Recipe Viewer")]
    public SimpleScrollSnap snap;
    public int groupOfContent;
    public Transform pageContent;
    public Transform paginationContent;
    public GameObject prefabPage;
    public GameObject prefabPagination;
    public GameObject[] prefabRecipeItem;
    public Transform recipeContentTemp;
    public List<RecipeItem> recipeItems = new List<RecipeItem>();
    public List<GameObject> pageLists = new List<GameObject>();
    public List<GameObject> paginationLists = new List<GameObject>();

    private void Awake() {
        instance = this;
    }

    // ------ Recipe Management ------

    public void InitRecipe() {
        foreach (RecipeData item in GameSystemManager.instance.recipeTable.table) {
            GameObject recipe = Instantiate(prefabRecipeItem[(int)item.recipeRarity], recipeContentTemp);
            RecipeItem recipeItem = recipe.GetComponent<RecipeItem>();
            recipeItem.SetItem(item);
            recipeItem.GetComponent<Toggle>().group = pageContent.GetComponent<ToggleGroup>();
            recipeItems.Add(recipeItem);
        }

        SetPage(recipeItems);
    }

    public void SetPage(List<RecipeItem> list) {
        Debug.Log("List Count : " + list.Count);

        ClearViewer();
        double pageNum = Mathf.CeilToInt(1.0f * list.Count / groupOfContent);
        Debug.Log("page Num : " + pageNum);

        for (int i = 0; i < pageNum; i++) {
            GameObject page = Instantiate(prefabPage, pageContent);
            pageLists.Add(page);

            GameObject pagiantion = Instantiate(prefabPagination, paginationContent);
            pagiantion.GetComponent<Toggle>().group = paginationContent.GetComponent<ToggleGroup>();
            paginationLists.Add(pagiantion);
        }

        for (int i = 0; i< list.Count; i++) {
            int inPage = (int)(1.0f * i / groupOfContent);
            Debug.Log("page : " + inPage);
            list[i].transform.SetParent(pageLists[inPage].transform);
        }

        snap.Setup();
        snap.enabled = true;
    }
    public void ClearViewer() {
        snap.enabled = false;

        foreach(var item in recipeItems) {
            item.transform.SetParent(recipeContentTemp);
        }

        int count = pageContent.childCount;
        for (int i = 0; i < count; i++) {
            DestroyImmediate(pageContent.GetChild(0).gameObject);
        }
        pageLists.Clear();

        int count2 = paginationContent.childCount;
        for (int i = 0; i < count2; i++) {
            DestroyImmediate(paginationContent.GetChild(0).gameObject);
        }
        paginationLists.Clear();
    }

    public List<RecipeItem> GetListRecipe(int filter, string search) {
        List<RecipeItem> newListFilter = recipeItems;
        if (filter > -1) {
            newListFilter = recipeItems.FindAll(x => (int)x.recipeData.recipeRarity == filter);
        }

        List<RecipeItem> newListSearch = newListFilter;
        if(search != "") {
            newListSearch = newListFilter.FindAll(x => x.recipeData.recipeName.Contains(search));
        }

        return newListSearch;
    }

    // ------ Cooking Management ------
    public bool CanCook(RecipeData recipe) {
        foreach (var ing in recipe.ingredientRequirement) {
            if (!playerInventory.HasItem(ing.ingredient.ingredientName, ing.amount))
                return false;
        }
        return true;
    }

    public void CookRecipe(RecipeData recipe) {
        if (CanCook(recipe)) {
            foreach (var ing in recipe.ingredientRequirement) {
                playerInventory.UseItem(ing.ingredient.ingredientName, ing.amount);
            }
            Debug.Log("Cooked: " + recipe.recipeName);
        } else {
            Debug.Log("Not enough ingredients for " + recipe.recipeName);
        }
    }
}
