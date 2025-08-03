using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnhanceUIController : MonoBehaviour
{
    [SerializeField] private HeroCard heroCard;
    [SerializeField] private UpgradeView upgradeView;
    [SerializeField] private GameObject heroCardview;
    [SerializeField] private RectTransform heroCardScrollview;
    [SerializeField] private List<HeroCard> heroCards;

    void Start()
    {
        for(int i=0;i< HeroManager.instance.heroDatas.Count; i++)
        {
            var heroCardIns = Instantiate(heroCard, heroCardScrollview);
            var hero = HeroManager.instance.heroDatas[i];
            heroCardIns.SetData(hero.id, upgradeView, heroCardview, hero.level, hero.hp,hero.mp, hero.heroCardImage);
            heroCards.Add(heroCardIns);
        }
     
    }

    private void OnEnable()
    {
        if (heroCards.Count > 0)
        {
            for (int i = 0; i < HeroManager.instance.heroDatas.Count; i++)
            {

                var hero = HeroManager.instance.heroDatas[i];
                heroCards[i].SetData(hero.id, upgradeView, heroCardview, hero.level, hero.hp,hero.mp,hero.heroCardImage);
               
            }
        }
    }
}
