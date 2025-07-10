using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public struct HeroCardSprite {
    public int id;
    public Sprite img;
}

public class ChangeHeroUIController : MonoBehaviour
{
    [SerializeField] private StageHeroController StageHeroController;
    [SerializeField] private ChangeHeroButton changeHeroButton;
    [SerializeField] private List<ChangeHeroButton> changeHeroButtons;
    [SerializeField] private RectTransform buttonParent;
    [SerializeField] private List<HeroCardSprite> heroCardSprites;

    public void CreateButtons()
    {
        int index = 0;

        foreach (var heroData in HeroManager.instance.heroDatas)
        {
            if (!heroData.isUnlock) continue;

            Sprite sprite = heroCardSprites.Find(h => h.id == heroData.id).img;

            if (index < changeHeroButtons.Count)
            {
                changeHeroButtons[index].SetData(heroData.id, heroData.level, heroData.hp,
                    heroData.mp, sprite, (id) =>
                    {
                        StageHeroController.SetHeroID(id); 
                        UpdateSelection(id);              
                    });
                changeHeroButtons[index].SetSelected(heroData.id == StageHeroController.currentId); 
                changeHeroButtons[index].gameObject.SetActive(true);
            }
            else
            {
                var button = Instantiate(changeHeroButton, buttonParent);
                button.SetData(heroData.id, heroData.level, heroData.hp, heroData.mp,
                    sprite, (id) =>
                    {
                        StageHeroController.SetHeroID(id); 
                        UpdateSelection(id);            
                    });
                button.SetSelected(heroData.id == StageHeroController.currentId);
                changeHeroButtons.Add(button);
            }

            index++;
        }
        for (int i = index; i < changeHeroButtons.Count; i++)
        {
            changeHeroButtons[i].gameObject.SetActive(false);
        }
        buttonParent.anchoredPosition = new Vector2(155,buttonParent.anchoredPosition.y);
    }
    public void UpdateSelection(int selectedHeroId)
    {
        foreach (var button in changeHeroButtons)
        {
            bool isSelected = (button.heroID == selectedHeroId);
            button.SetSelected(isSelected);
            PlayerController.instance.SetCurrentData(StageHeroController.GetCurrentHeroData());
        }
    }


    private void OnEnable()
    {
        CreateButtons();
    }

  
}
