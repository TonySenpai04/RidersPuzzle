using UnityEngine;

internal class SkulzSkill : BaseSkill
{
   
    public SkulzSkill(GridController gridController, int skillAmount, int id)
    {
        this.gridController = gridController;
        this.maxSkillAmount = skillAmount;
        this.skillAmount = skillAmount;

        this.id = id;
    }

    public override void ActivateSkill()
    {
        if (skillAmount > 0)
        {
            SoundManager.instance.PlayHeroSFX(id);
            PlayerController.instance.hitPoint.Heal((int)PlayerController.instance.hitPoint.GetMaxHealth());
            skillAmount--;
        }
    }

 
}