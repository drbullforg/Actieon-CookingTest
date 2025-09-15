using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager: MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Money & Energy")]
    public int money;
    public int energy;
    public int maxEnergy = 20;
    public Slider energyBar;
    public TextMeshProUGUI energyText;

    [Header("Energy Regen")]
    public int regenAmount = 1;
    public int regenIntervalSeconds = 5;
    private DateTime lastRegenTime;

    [Header("Inventory")]
    public InventoryData inventoryData;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    [System.Serializable]
    private class SaveData
    {
        public int money;
        public int energy;
        public string lastRegenTime;
        public List<string> ingredientNames = new List<string>();
        public List<int> ingredientAmounts = new List<int>();
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        LoadPlayerData();
        InvokeRepeating(nameof(CheckEnergyRegen), 1f, 5f);

        PrintInventory();
    }

    // -------- Inventory Methods --------
    public void AddItem(string name, int amount) {
        if (!inventory.ContainsKey(name))
            inventory[name] = 0;
        inventory[name] += amount;

        SavePlayerData();
    }

    public bool UseItem(string name, int amount) {
        if (inventory.ContainsKey(name) && inventory[name] >= amount) {
            inventory[name] -= amount;
            SavePlayerData();
            return true;
        }
        return false;
    }

    public bool HasItem(string name, int amount) {
        return inventory.ContainsKey(name) && inventory[name] >= amount;
    }

    public void PrintInventory() {
        foreach (var item in inventory) {
            Debug.Log(item.Key + "x" + item.Value);
        }
    }

    // -------- Energy Methods --------
    public void AddEnergy(int amount) {
        energy = Mathf.Min(energy + amount, maxEnergy);
        UpdateEnergyBar();
    }

    public bool UseEnergy(int amount) {
        if (energy >= amount) {
            energy -= amount;
            UpdateEnergyBar();
            return true;
        }
        return false;
    }

    public bool HasEnergy(int amount) {
        return energy >= amount;
    }

    private void CheckEnergyRegen() {
        if (energy >= maxEnergy) {
            lastRegenTime = DateTime.Now;
            return;
        }

        TimeSpan elapsed = DateTime.Now - lastRegenTime;
        int minutesPassed = (int)elapsed.TotalSeconds;

        if (minutesPassed >= regenIntervalSeconds) {
            int times = minutesPassed / regenIntervalSeconds;
            int totalRegen = times * regenAmount;

            AddEnergy(totalRegen);
            lastRegenTime = DateTime.Now;
            SavePlayerData();

            Debug.Log($"Energy regenerated: +{totalRegen}, now {energy}/{maxEnergy}");
        }
    }

    public void UpdateEnergyBar() {
        energyBar.maxValue = maxEnergy;
        energyBar.value = energy;

        energyText.text = energy + "/" + maxEnergy;
        SavePlayerData();
    }

    // -------- Money Methods --------
    public void AddMoney(int amount) => money += amount;
    public bool SpendMoney(int amount) {
        if (money >= amount) {
            money -= amount;
            return true;
        }
        return false;
    }

    // -------- Save / Load --------
    public void SavePlayerData() {
        SaveData data = new SaveData {
            money = this.money,
            energy = this.energy,
            lastRegenTime = lastRegenTime.ToString()
        };

        foreach (var kvp in inventory) {
            data.ingredientNames.Add(kvp.Key);
            data.ingredientAmounts.Add(kvp.Value);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();

        Debug.Log("PlayerData Saved: " + json);
    }

    public void LoadPlayerData() {
        if (!PlayerPrefs.HasKey("PlayerData")) {
            energy = maxEnergy;
            lastRegenTime = DateTime.Now;

            LoadDataFromTable();
            return;
        }

        string json = PlayerPrefs.GetString("PlayerData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        this.money = data.money;
        this.energy = data.energy;
        this.lastRegenTime = DateTime.TryParse(data.lastRegenTime, out var parsed) ? parsed : DateTime.Now;

        inventory.Clear();
        for (int i = 0; i < data.ingredientNames.Count; i++) {
            inventory[data.ingredientNames[i]] = data.ingredientAmounts[i];
        }
        CheckEnergyRegen();
        UpdateEnergyBar();

        Debug.Log("PlayerData Loaded: " + json);
    }

    public void LoadDataFromTable() {
        foreach (var item in inventoryData.items) {
            inventory.Add(item.ingredient.ingredientName, item.amount);
        }
    }
}
