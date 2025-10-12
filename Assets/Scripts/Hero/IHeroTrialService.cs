using System.Collections.Generic;

public interface IHeroTrialService
{
    void StartTrial(List<DataHero> heroes, int heroId, int durationDays);
    void StartTrialSeconds(List<DataHero> _heroes, int heroId, int durationSeconds);
    void CheckTrials(List<DataHero> heroes);
    bool IsTrialHero(List<DataHero> heroes,int heroId);
    bool IsTrialExpired(List<DataHero> heroes, int heroId);
    string GetTrialRemainingTime(List<DataHero> heroes, int heroId);
    string GetTrialRemainingTime_Full(List<DataHero> _heroes, int heroId);
    string GetTrialRemainingTime_Short(List<DataHero> _heroes, int heroId);
    bool IsChangeTrials(List<DataHero> heroDatas);


}