using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


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
        SortObjects();
        LoadSeenObjects();
        SplitObjects();
    }
    private void SortObjects()
    {
        AllObjects.Sort((a, b) =>
        {
            if (a.id == 3000) return -1;
            if (b.id == 3000) return 1;
            return a.id.CompareTo(b.id);
        });
    }
    private void SplitObjects()
    {

        foreach (var obj in AllObjects)
        {
            if (obj.type== HiddenObject.ObjectType.PowerUp) 
            {
                powerUpObjects.Add(obj);
            }
            else if(obj.type == HiddenObject.ObjectType.Obstacle)
            {
                obstacleObjects.Add(obj);
            }
        }
    }
    public void  SetSeenObjectById(int id)
    {
        var obj = allObjects.FirstOrDefault(h => h.id == id);
        if (obj != null && !obj.isSeen)
        {
            obj.isSeen = true;
            obj.isFirstSeen = true;
            NotiManager.instance.ShowNotiRedDot("library");
            NotiManager.instance.ShowNotiRedDot("object");
            SaveSeenObjects();
        }
        // allObjects.FirstOrDefault(h=>h.id==id).isSeen = true;
        //SaveSeenObjects();
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
    public HiddenObject GetById(int id)
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
        List<int> seenObjectIds = allObjects.Where(h => h.isSeen).Select(h => h.id).ToList();
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
            foreach (int id in data.seenObjectIds)
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
