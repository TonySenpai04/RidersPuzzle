using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLibraryController : MonoBehaviour
{
    [SerializeField] private HeroLibrary HeroLibraryPrefabs;
    [SerializeField] private Transform content;
    [SerializeField] private HeroView HeroView;
    [SerializeField] private GameObject heroParent;
    void Start()
    {
        int count=HeroManager.instance.heroDatas.Count;
        for(int i=0;i<count; i++)
        {
            HeroLibrary hero = Instantiate(HeroLibraryPrefabs, content.transform);
            hero.SetHero(HeroManager.instance.heroDatas[i].id, HeroManager.instance.heroDatas[i].heroImage,
                HeroView, heroParent);
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
