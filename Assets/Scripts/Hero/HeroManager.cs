using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct DataHero
{
    public int id;
    public int hp;
    public Sprite icon;
    public bool isUnlock;
    public Sprite heroImage;
}
public class HeroManager : MonoBehaviour
{
     public List<DataHero> heroDatas;
     public static HeroManager instance;
     void Awake()
     {
        instance = this;
     }


}
