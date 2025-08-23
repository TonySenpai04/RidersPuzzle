using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectionController : IHeroSelection
{
    private List<int> tempSelectedHeroes = new List<int>();
    private List<int> selectedHeroes = new List<int>(); // ✅ giữ hero đã confirm
    private List<HeroFusionButton> heroButtons=new List<HeroFusionButton>();
    private HeroFusionButton heroButtonPrefab;
    private Transform heroButtonContainer;
    private Button confirmButton;

    public HeroSelectionController( HeroFusionButton prefab, Transform container, Button confirmBtn)
    {
        heroButtonPrefab = prefab;
        heroButtonContainer = container;
        confirmButton = confirmBtn;
    }

    public void CreateHeroButtons()
    {
        selectedHeroes.RemoveAll(id =>
        {
            var hero = HeroManager.instance.GetHero(id);
            return !hero.Value.isUnlock && !hero.Value.isTrial;
        });
        tempSelectedHeroes.RemoveAll(id =>
        {
            var hero = HeroManager.instance.GetHero(id);
            return !hero.Value.isUnlock && !hero.Value.isTrial;
        });
        var unlockHeros  = HeroManager.instance.heroDatas
            .Where(h => (h.isUnlock || h.isTrial) )
            .ToList();
        int index = 0;

        foreach (var heroData in unlockHeros)
        {
            if (heroData.currentMP <= 0) continue;

            HeroFusionButton btn;
            if (index < heroButtons.Count)
            {
                btn = heroButtons[index];
            }
            else
            {
                btn = GameObject.Instantiate(heroButtonPrefab, heroButtonContainer);
                heroButtons.Add(btn);
            }

            btn.SetData(heroData.id, heroData.heroCardImage, heroData.level, heroData.hp, heroData.mp, SelectHero);
            btn.SetHighlight(selectedHeroes.Contains(heroData.id)); 
            btn.gameObject.SetActive(true);
            index++;
        }

        for (int i = index; i < heroButtons.Count; i++)
        {
            heroButtons[i].gameObject.SetActive(false);
        }
        confirmButton.gameObject.SetActive(tempSelectedHeroes.Count >= 3);

    }

    public void SelectHero(int heroId)
    {
        if (!tempSelectedHeroes.Contains(heroId))
        {
            if (tempSelectedHeroes.Count >= 5) return;
            tempSelectedHeroes.Add(heroId);
        }
        else
        {
            tempSelectedHeroes.Remove(heroId);
        }

        foreach (var btn in heroButtons)
        {
            btn.SetHighlight(tempSelectedHeroes.Contains(btn.GetID()));
        }

        confirmButton.gameObject.SetActive(tempSelectedHeroes.Count >= 3);
    }

    public void ConfirmSelection()
    {
        selectedHeroes = new List<int>(tempSelectedHeroes);
    }

    public List<int> GetSelectedHeroes() => new List<int>(selectedHeroes);

    public void ClearSelection()
    {
        tempSelectedHeroes.Clear();
        selectedHeroes.Clear();
        foreach (var btn in heroButtons)
        {
            btn.SetHighlight(false);
        }
    }
}
