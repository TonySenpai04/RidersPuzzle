using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[Serializable]
public class ResourceData
{
    public int resourceType;  // 0 = coin, 1 = avatar, 2 = hero exp, 3 = mastery
    public int resourceId;    // ví dụ: 1001 = EXP của hero 1001
    public int quantity;

    public ResourceData(int type, int id, int qty)
    {
        resourceType = type;
        resourceId = id;
        quantity = qty;
    }
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    // Key = (type, id) -> quantity
    private Dictionary<(int, int), int> resourceDict ;

    void Awake()
    {
        Instance = this;
        resourceDict = new Dictionary<(int, int), int>
         {
                { (0,    1),    10000000 },
                { (0,    2),    10000000 },
                { (2, 1001),    10000000 },
                { (2, 1002),    10000000 },
                { (2, 1003),    10000000 },
                { (3, 1001),    10000000 },
                { (3, 1002),    10000000 },
                { (3, 1003),    10000000 }
        };


       
       // LoadResources(); // load từ file hoặc Firebase
    }

    public int GetQuantity(int type, int id)
    {
        return resourceDict.TryGetValue((type, id), out int qty) ? qty : 0;
    }

    public bool HasEnough(int type, int id, int amount)
    {
        return GetQuantity(type, id) >= amount;
    }

    public void AddResource(int type, int id, int amount)
    {
        var key = (type, id);
        if (!resourceDict.ContainsKey(key))
            resourceDict[key] = 0;

        resourceDict[key] += amount;
        SaveResources();
    }

    public bool ConsumeResource(int type, int id, int amount)
    {
        if (!HasEnough(type, id, amount)) return false;

        resourceDict[(type, id)] -= amount;
        SaveResources();
        return true;
    }

    public void SaveResources()
    {
        string json = JsonUtility.ToJson(new ResourceSaveWrapper(resourceDict));
        File.WriteAllText(Application.persistentDataPath + "/resources.json", json);
    }

    public void LoadResources()
    {
        string path = Application.persistentDataPath + "/resources.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<ResourceSaveWrapper>(json);
            resourceDict = wrapper.ToDictionary();
        }
    }
}
[Serializable]
public class ResourceSaveWrapper
{
    public List<ResourceData> resources = new();

    public ResourceSaveWrapper(Dictionary<(int, int), int> dict)
    {
        foreach (var kv in dict)
        {
            resources.Add(new ResourceData(kv.Key.Item1, kv.Key.Item2, kv.Value));
        }
    }

    public Dictionary<(int, int), int> ToDictionary()
    {
        var dict = new Dictionary<(int, int), int>();
        foreach (var res in resources)
        {
            dict[(res.resourceType, res.resourceId)] = res.quantity;
        }
        return dict;
    }
}

