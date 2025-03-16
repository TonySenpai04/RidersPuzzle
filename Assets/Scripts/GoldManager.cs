using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class GoldManager : MonoBehaviour
{
    public string goldPath=>Path.Combine(Application.persistentDataPath, "GoldData.json");
    public int currentGold;
    public static GoldManager instance;
    private void Awake()
    {
        instance = this;
        LoadLocal();
    }
    public void SaveGold()
    {
        var data = new GoldData { amount = currentGold };
        string json=JsonUtility.ToJson(data);
        File.WriteAllText(goldPath, json);
        FirebaseDataManager.Instance.SaveData(
               LevelManager.instance.GetAllLevelComplete(),currentGold,
                     SaveGameManager.instance.LoadAllProgress(), HeroManager.instance.GetUnlockHeroID());

    }
    public void LoadCloudData()
    {
        FirebaseDataManager.Instance.LoadPlayerData((loadedData) =>
        {
            currentGold = loadedData.gold;
        });


    }
    public void LoadLocal()
    {
        if (File.Exists(goldPath))
        {
            string json = File.ReadAllText(goldPath);
            var gold = JsonUtility.FromJson<GoldData>(json);
            this.currentGold = gold.amount;
        }
        else
        {
            this.currentGold = 0;
        }
    }
    public void AddGold(int amount)
    {
        if (amount > 0)
        {
            currentGold += amount;
            SaveGold();
        }
        else
        {
            Console.WriteLine("Số vàng thêm phải lớn hơn 0.");
        }
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.G))
        {
            AddGold(1000000);
        }
    }
    public void SpendGold(int amount)
    {
        if (amount > 0 && currentGold >= amount)
        {
            currentGold -= amount;
            SaveGold();



        }
        if(currentGold < 0)
        {
            currentGold = 0;
        }
 
    }

    public int GetGold()
    {
        return currentGold;
    }

    public void ResetGold(int newGoldAmount)
    {
        currentGold = newGoldAmount;
        SaveGold();
    }
}
