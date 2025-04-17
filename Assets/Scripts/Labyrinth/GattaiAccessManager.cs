using System.Collections.Generic;
using UnityEngine;

public class GattaiAccessManager : MonoBehaviour
{
    public static GattaiAccessManager instance;

    public int unlockStage = 100;
    public int unlockHeroCount = 3;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Kiểm tra điều kiện mở khóa
    public bool IsGattaiUnlocked()
    {
        int ownedHeroCount = HeroManager.instance.GetUnlockHero().Count;
        int maxStageReached = LevelManager.instance.GetAllLevelComplete();

        return ownedHeroCount >= unlockHeroCount && maxStageReached >= unlockStage;
    }

    // Mở tính năng Gattai nếu thỏa điều kiện
    public bool TryUnlockGattai()
    {
        if (IsGattaiUnlocked())
        {
            Debug.Log("Gattai đã được mở khóa!");
            return true;
        }
        else
        {
            Debug.Log("Chưa đủ điều kiện để mở khóa Gattai.");
            return false;
        }
    }

    
}
