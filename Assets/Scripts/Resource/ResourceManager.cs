using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
    public List<ResourceInfoData> resourceInfos;
    // Key = (type, id) -> quantity
    private Dictionary<(int, int), int> resourceDict ;

    void Awake()
    {
        Instance = this;
        resourceDict = new Dictionary<(int, int), int>
         {
                { (0,    1),    0 },
                { (0,    2),    0 },
                { (2, 1001),    0 },
                { (2, 1002),    0 },
                { (2, 1003),    0 },
                { (2, 1004),    0 },
                { (3, 1001),    0 },
                { (3, 1002),    0 },
                { (3, 1003),    0 },
                { (3, 1004),    0 }
        };

    
    }
    private async void Start()
    {
        await Task.Delay(3500);
        LoadResources();
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
        if (key ==(0, 1))
        {
            GoldManager.instance.AddGold(amount);
        }
        SaveResources();
    }
    public void TestAdd()
    {
        SetResource(0,1,100000000);
        SetResource(2, 1001, 100000000);
        SetResource(2, 1002, 100000000);
        SetResource(2, 1003, 100000000);
        SetResource(2, 1004, 100000000);
        SetResource(3, 1001, 100000000);
        SetResource(3, 1002, 100000000);
        SetResource(3, 1003, 100000000);
        SetResource(3, 1004, 100000000);
    }
    public void SetResource(int type, int id, int amount)
    {
        var key = (type, id);
        if (!resourceDict.ContainsKey(key))
            resourceDict[key] = 0;

        resourceDict[key] = amount;
        if (key == (0, 1))
        {
            GoldManager.instance.ResetGold(amount);
        }
        SaveResources();
    }
    public bool ConsumeResource(int type, int id, int amount)
    {
        if (!HasEnough(type, id, amount)) return false;

        resourceDict[(type, id)] -= amount;
        if ((type, id) == (0, 1))
        {
            GoldManager.instance.SpendGold(amount);
        }
        SaveResources();
        return true;
    }
    public Sprite GetIconForResource(int type, int id)
    {
        var info = resourceInfos.Find(r => r.resourceType == type && r.resourceId == id);
        return info != null ? info.icon : null;
    }
    public void SaveResources()
    {
        string json = JsonUtility.ToJson(new ResourceSaveWrapper(resourceDict));
        File.WriteAllText(Application.persistentDataPath + "/resources.json", json);
        SaveResourcesToFirebase();
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
        LoadResourcesFromFirebase();
        SetResource(0,1,  GoldManager.instance.GetGold());
        Debug.Log("da load");

    }
    public void SaveResourcesToFirebase()
    {
        string json = JsonUtility.ToJson(new ResourceSaveWrapper(resourceDict));
        if (FirebaseDataManager.Instance.GetCurrentUser() == null)
        {
            Debug.Log("❌ User not logged in, can't save to Firebase.");
            return;
        }
        string userId = FirebaseDataManager.Instance.GetCurrentUser().UserId;

        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(userId)
            .Child("resources")
            .SetRawJsonValueAsync(json)
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log("Resources saved to Firebase");
                }
                else
                {
                    Debug.LogError("Failed to save resources: " + task.Exception);
                }
            });
    }
    public void LoadResourcesFromFirebase()
    {
        if (FirebaseDataManager.Instance.GetCurrentUser() == null)
        {
            Debug.Log("❌ User not logged in, can't save to Firebase.");
            return;
        }
        string userId = FirebaseDataManager.Instance.GetCurrentUser().UserId;


        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(userId)
            .Child("resources")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully && task.Result.Exists)
                {
                    string json = task.Result.GetRawJsonValue();
                    var wrapper = JsonUtility.FromJson<ResourceSaveWrapper>(json);
                    resourceDict = wrapper.ToDictionary();
                    Debug.Log("Resources loaded from Firebase");
                }
                else
                {
                    Debug.LogWarning("No resource data found on Firebase or failed to load.");
                }
            });
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


[System.Serializable]
public class ResourceInfoData
{
    public int resourceType;
    public int resourceId;
    public string resourceName;
    public Sprite icon;

    public ResourceInfoData(int type, int id, string name, Sprite icon)
    {
        this.resourceType = type;
        this.resourceId = id;
        this.resourceName = name;
        this.icon = icon;
    }

    // Optional: Key để tra nhanh
    public (int, int) GetKey() => (resourceType, resourceId);
}
