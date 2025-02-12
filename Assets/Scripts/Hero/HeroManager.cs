using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public struct DataHero
{
    public int id;
    public string name;
    public int hp;
    public Sprite icon;
    public bool isUnlock;
    public Sprite heroImage;
    public string skillDescription;
}
public class HeroManager : MonoBehaviour
{
     public List<DataHero> heroDatas;
     public static HeroManager instance;
     void Awake()
     {
        instance = this;
     }
    public DataHero? GetHero(int id)
    {
        return heroDatas.FirstOrDefault(h => h.id == id);
    }
    public int HeroOwnedQuantity()
    {
        int unlockedCount = heroDatas.Count(hero => hero.isUnlock);
        return unlockedCount;
    }


}
