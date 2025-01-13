using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float bounceHeight = 0.1f; 
    [SerializeField] private float bounceSpeed = 0.5f; 
    private float originalY; 
    private bool isBouncing = true;
    public Transform player;
    public void SetY(Transform transform)
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        if (isBouncing)
        {
            float elapsedTime = Mathf.PingPong(Time.time * bounceSpeed, bounceHeight);
            player.transform.position = new Vector3(player.transform.position.x, originalY + elapsedTime, transform.position.z);
        }
    }
    public void StartBounce()
    {
        isBouncing = true;
    }
    public void StopBounce()
    {
        isBouncing = false;
        transform.position = new Vector3(transform.position.x, originalY, transform.position.z); // Trả về vị trí gốc
    }
    public void SetBounceParams(float height, float speed)
    {
        bounceHeight = height;
        bounceSpeed = speed;
    }
}
