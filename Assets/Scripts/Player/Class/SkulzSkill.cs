using UnityEngine;

internal class SkulzSkill : ISkill
{
    private int maxSkillAmount;
    private int skillAmount;
    private GridController gridController;
    private int id;
    public SkulzSkill(GridController gridController, int skillAmount, int id)
    {
        this.gridController = gridController;
        this.maxSkillAmount = skillAmount;
        this.skillAmount = skillAmount;

        this.id = id;
    }

    public void ActivateSkill()
    {
        if (skillAmount > 0)
        {
            SoundManager.instance.PlayHeroSFX(id);
            PlayerController.instance.hitPoint.Heal((int)PlayerController.instance.hitPoint.GetMaxHealth());
            skillAmount--;
        }
    }

    public void IncreaseUses(int amount)
    {
        skillAmount += amount;
        if (skillAmount > maxSkillAmount)
        {
            skillAmount = maxSkillAmount;
        }


    }
    public int GetNumberOfSkill()
    {
        return skillAmount;
    }
    public void SetNumberOfSkill(int amount)
    {
        this.skillAmount = amount;
    }
}