using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HiddenObject : MonoBehaviour
{
    public string id;
    public ObjectType type;

    public enum ObjectType
    {
        PowerUp,   
        Obstacle     
    }
    public virtual void Start()
    {
       // Show();
    }


    public virtual void Hide()
    {
        this.gameObject.SetActive(false);

    }
    public virtual void Show()
    {
        this.gameObject.SetActive(true);
    }
    public virtual void ActiveSkill()
    {
        Debug.Log("Skill Activated!");
    }
    public virtual void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    
}
