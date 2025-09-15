using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Image thumbnail;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI nameText2;

    public void SetItem(RecipeData _recipeData) {

        thumbnail.sprite = _recipeData.recipeIcon;
        thumbnail.SetNativeSize();

        nameText.text = nameText2.text = _recipeData.recipeName;

        gameObject.SetActive(true);
    }
}
