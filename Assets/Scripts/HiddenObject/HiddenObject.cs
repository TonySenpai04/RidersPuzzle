using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HiddenObject : MonoBehaviour
{
    public string id;
    public ObjectType type;
    public bool isDestroying = false;

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
        if (!isDestroying) // Đảm bảo chỉ gọi một lần
        {
            isDestroying = true; // Đặt trạng thái là đang biến mất
            StartCoroutine(ShrinkAndDestroy(0.5f));
        }
       // StartCoroutine(ShrinkAndDestroy(0.5f));
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
        GetComponentInParent<BoxCollider2D>().enabled = true;

        Destroy(this.gameObject);
    }
    public virtual void PlaySFX()
    {
        SoundManager.instance.StopSFX();
        if (type == ObjectType.PowerUp)
        {
            SoundManager.instance.PlaySFX("Touch Positive Object");
        }
        else
        {
            SoundManager.instance.PlaySFX("Touch Negative Object");
        }
    }


}
