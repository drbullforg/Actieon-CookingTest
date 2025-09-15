using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickShopManager : MonoBehaviour
{
    public IngredientData ingredientData;
    public Image thumbnail;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI nameText2;
    public TextMeshProUGUI amount;

    public void SetItem(IngredientData _ingredientData) {
        ingredientData = _ingredientData;
        thumbnail.sprite = ingredientData.ingredientIcon;
        thumbnail.SetNativeSize();

        nameText.text = nameText2.text = ingredientData.ingredientName;
        amount.text = "มีอยู่ " + PlayerManager.instance.inventory[ingredientData.ingredientName].ToString();
        gameObject.SetActive(true);
    }

    public void BuyIngredient() {
        PlayerManager.instance.AddItem(ingredientData.ingredientName, 1);
        amount.text = "มีอยู่ " + PlayerManager.instance.inventory[ingredientData.ingredientName].ToString();
        IngredientManager.instance.UpdateAmount();
    }
}
