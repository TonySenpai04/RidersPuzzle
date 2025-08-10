using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeroSelection
{
    void CreateHeroButtons();
    void SelectHero(int heroId);
    List<int> GetSelectedHeroes();
    void ClearSelection();
    void ConfirmSelection();
}
