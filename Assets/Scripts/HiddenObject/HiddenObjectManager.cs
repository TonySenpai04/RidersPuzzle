using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjectManager : MonoBehaviour
{
    [SerializeField] private List<HiddenObject> allObjects;
    [SerializeField] private List<HiddenObject> powerUpObjects;
    [SerializeField] private List<HiddenObject> obstacleObjects;
    public static HiddenObjectManager instance;
    private void Awake()
    {
        instance = this;
        SplitObjects();
    }
    private void SplitObjects()
    {

        foreach (var obj in allObjects)
        {
            if (obj.type== HiddenObject.ObjectType.PowerUp) // Điều kiện phân loại PowerUp
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
        foreach (var obj in allObjects)
        {
            if (obj.id == id) // So sánh id của đối tượng với id truyền vào
            {
                return obj; // Trả về đối tượng tìm thấy
            }
        }
        return null;
    }


    public HiddenObject GetRandomObstacle()
    {
        if (obstacleObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, obstacleObjects.Count);
            return obstacleObjects[randomIndex];
        }
        return null; // Nếu không có Obstacle nào
    }
}
