using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class SeenObjectsData
{
    public List<string> seenObjectIds;

    public SeenObjectsData(List<string> ids)
    {
        seenObjectIds = ids;
    }
}

public class HiddenObjectManager : MonoBehaviour
{
    [SerializeField] private List<HiddenObject> allObjects;
    [SerializeField] private List<HiddenObject> powerUpObjects;
    [SerializeField] private List<HiddenObject> obstacleObjects;
    public static HiddenObjectManager instance;

    public List<HiddenObject> AllObjects { get => allObjects; set => allObjects = value; }

    private void Awake()
    {
        instance = this;
        LoadSeenObjects();
        SplitObjects();
    }
    private void SplitObjects()
    {

        foreach (var obj in AllObjects)
        {
            if (obj.type== HiddenObject.ObjectType.PowerUp) 
            {
                powerUpObjects.Add(obj);
            }
            else
            {
                obstacleObjects.Add(obj);
            }
        }
    }
    public void  SetSeenObjectById(string id)
    {
         allObjects.FirstOrDefault(h=>h.id==id).isSeen = true;
        SaveSeenObjects();
    }
    public List<HiddenObject> GetSeenObject()
    {
        return allObjects.Where(h => h.isSeen).ToList();

    }
    public HiddenObject GetRandomPowerUp()
    {
        if (powerUpObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, powerUpObjects.Count);
            return powerUpObjects[randomIndex];
        }
        return null; 
    }
    public HiddenObject GetById(string id)
    {
        foreach (var obj in AllObjects)
        {
            if (obj.id == id) 
            {
                return obj; 
            }
        }
        return null;
    }

    public void SaveSeenObjects()
    {
        List<string> seenObjectIds = allObjects.Where(h => h.isSeen).Select(h => h.id).ToList();
        string json = JsonUtility.ToJson(new SeenObjectsData(seenObjectIds));
        File.WriteAllText(Application.persistentDataPath + "/seenObjects.json", json);
    }
    public void LoadSeenObjects()
    {
        string path = Application.persistentDataPath + "/seenObjects.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SeenObjectsData data = JsonUtility.FromJson<SeenObjectsData>(json);
            foreach (string id in data.seenObjectIds)
            {
                SetSeenObjectById(id);
            }
        }
    }

    public HiddenObject GetRandomObstacle()
    {
        //foreach(var obj in allObjects)
        //{
        //    if (obj.id == "2007")
        //    {
        //        return obj;
        //    }
        //}
        if (obstacleObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, obstacleObjects.Count);
            return obstacleObjects[randomIndex];
        }
        return null; 
    }
    public int ObjectQuantity()
    {
        return AllObjects.Count;
    }
}
