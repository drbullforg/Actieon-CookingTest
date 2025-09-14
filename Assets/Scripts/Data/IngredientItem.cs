using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientItem : MonoBehaviour
{
    public IngredientData ingredientData;
    public int amount;
    public Image thumbnail;
    public TextMeshProUGUI nameText;

    public void SetItem(IngredientData _ingredientData, int _amount) {
        ingredientData = _ingredientData;
        amount = _amount;
        thumbnail.sprite = ingredientData.ingredientIcon;
        thumbnail.SetNativeSize();

        UpdateAmount();
    }

    public void UpdateAmount() {
        string inventory;
        if (PlayerManager.instance.inventory[ingredientData.ingredientName] < amount) {
            inventory = "<color=red>" + PlayerManager.instance.inventory[ingredientData.ingredientName] + "</color>";
        } else {
            inventory = PlayerManager.instance.inventory[ingredientData.ingredientName].ToString();
        }
        nameText.text = inventory + "/" + amount;
    }
}
