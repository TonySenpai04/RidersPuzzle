using System.Collections.Generic;

public interface IHeroProgressService
{
    void SaveProgress(List<DataHero> heroes);
    void LoadProgress(List<DataHero> heroes);
    void SaveProgressToFirebase(List<DataHero> heroes);
    void LoadProgressFromFirebase(List<DataHero> heroes);
}