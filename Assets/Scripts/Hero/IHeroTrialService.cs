using System.Collections.Generic;

public interface IHeroTrialService
{
    void StartTrial(List<DataHero> heroes, int heroId, int durationDays);
    void CheckTrials(List<DataHero> heroes);
    bool IsTrialHero(List<DataHero> heroes,int heroId);
    bool IsTrialExpired(List<DataHero> heroes, int heroId);
    string GetTrialRemainingTime(List<DataHero> heroes,int heroId);
}