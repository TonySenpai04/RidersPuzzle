using System.Collections.Generic;

public class FusionService : IFusionService
{
    public DataHero FuseHeroes(List<int> heroIds, ISkill skill)
    {
        int totalHP = 0;
        int totalId = 0;

        foreach (var heroID in heroIds)
        {
            var hero = HeroManager.instance.GetHero(heroID).Value;
            totalHP += hero.hp;
            totalId += hero.id;
        }

        SkillManager.instance.AddSkillPVE(totalId, skill);
        SkillManager.instance.SetSkillId(totalId);

        return new DataHero { hp = totalHP, id = totalId };
    }
}
