using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class NewBoughtHeroManager : MonoBehaviour,INewBoughtHero
{
    public static NewBoughtHeroManager instance;

    private List<int> newBoughtHeroes = new List<int>();
    private string filePath;

    private void Awake()
    {
        instance = this;
        filePath = Application.persistentDataPath + "/newBoughtHeroes.json";
        LoadData();
    }

    public void AddNewHero(int id)
    {
        if (!newBoughtHeroes.Contains(id))
        {
            newBoughtHeroes.Add(id);
            SaveData();
        }
    }

    public void RemoveHero(int id)
    {
        if (newBoughtHeroes.Contains(id))
        {
            newBoughtHeroes.Remove(id);
            SaveData();
        }
    }

    public bool IsNewHero(int id)
    {
        return newBoughtHeroes.Contains(id);
    }

    public bool AllSeen()
    {
        return newBoughtHeroes.Count == 0;
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(new NewBoughtHeroData(newBoughtHeroes));
        File.WriteAllText(filePath, json);
    }

    private void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var data = JsonUtility.FromJson<NewBoughtHeroData>(json);
            newBoughtHeroes = data.newBoughtHeroIds ?? new List<int>();
        }
    }
}
[System.Serializable]
public class NewBoughtHeroData
{
    public List<int> newBoughtHeroIds;

    public NewBoughtHeroData(List<int> ids)
    {
        newBoughtHeroIds = ids;
    }
}
