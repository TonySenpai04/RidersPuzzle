using System.Collections;
using System.Collections.Generic;
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
