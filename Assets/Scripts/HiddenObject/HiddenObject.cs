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
        StartCoroutine(ShrinkAndDestroy(0.5f));
    }
    public IEnumerator ShrinkAndDestroy(float duration)
    {
        Vector3 initialScale = transform.localScale; 
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration); 
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, progress); 
            yield return null;
        }

        Destroy(this.gameObject);
    }


}
