using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : ISkill
{
    protected int maxSkillAmount;
    protected int skillAmount;
    protected GridController gridController;
    protected int id;


    public virtual void ActivateSkill()
    {
        
    }

    public virtual void IncreaseUses(int amount)
    {
        skillAmount += amount;
        if (skillAmount > maxSkillAmount)
        {
            skillAmount = maxSkillAmount;
        }


    }
    public virtual int GetNumberOfSkill()
    {
        return skillAmount;
    }
    public virtual void SetNumberOfSkill(int amount)
    {
        this.skillAmount = amount;
    }
}

