using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public float bounceHeight = 0.1f;  // Độ cao nhún
    public float bounceSpeed = 2f;     // Tốc độ nhún

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;  // Lưu vị trí ban đầu của nhân vật
        StartCoroutine(BounceEffect());  // Bắt đầu hiệu ứng nhún
    }
   void OnEnable() { 
        StopAllCoroutines();
        StartCoroutine(BounceEffect());
    }
    IEnumerator BounceEffect()
    {
        while (true)
        {
            // Tạo hiệu ứng nhún lên xuống
            float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
            transform.position = new Vector3(originalPosition.x, originalPosition.y + newY, originalPosition.z);
            yield return null;
        }
    }
    public void SetPos(Transform transform)
    {
        originalPosition= transform.position;
    }
}
