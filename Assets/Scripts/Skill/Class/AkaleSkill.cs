using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkaleSkill : BaseSkill
{
    public AkaleSkill(GridController gridController, int skillAmount, int id)
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
            float hp = PlayerController.instance.hitPoint.GetCurrentHealth();
            float move= PlayerController.instance.movementController.numberOfMoves.GetCurrentMove();
            if (hp < move)
            {
                PlayerController.instance.hitPoint.Heal((int)move);

            }
            else
            {
                PlayerController.instance.movementController.numberOfMoves.IncreaseMove((int)hp);
            }
            skillAmount--;
        }
    }


}
