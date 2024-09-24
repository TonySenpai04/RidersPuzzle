using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HiddenObject : MonoBehaviour
{
    public string id;
    public virtual void Start()
    {
        Hide();
    }


    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public virtual void ActiveSkill()
    {
        Debug.Log("Skill Activated!");
    }
}
