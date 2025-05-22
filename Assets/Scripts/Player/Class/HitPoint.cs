using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : IHitPoint, IImmortal
{
    private float health;
    private float currentHealth;
    public bool isImmortal;
    private int moveCount; 
    private int maxMoveCount = 3;
    private bool invincibleOnTrigger = false; 
    private int invincibleMoveCount = 0;
    private int maxInvincibleMoveCount = 3;

    public HitPoint(float health)
    {
        this.health = health;
        this.currentHealth = health;
        isImmortal = false;
        moveCount = 0;
    }
    public void Heal(int amount)
    {
        this.currentHealth += amount;
        if (currentHealth > health)
            currentHealth = health;
    }
    public float GetMaxHealth()
    {
        return health;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float value)
    {
        this.health = value;
        this.currentHealth = value;
    }
    public void ActivateImmortalEffect()
    {
        if (!isImmortal)
        {
            isImmortal = true;
            moveCount = 0;
            Debug.Log("Tao có khiêng");
        }

    }
    public void ActivateTriggerInvincibility()
    {
        if (invincibleOnTrigger)
            return;
        invincibleOnTrigger = true;
        invincibleMoveCount = 0;
        Debug.Log("Đang trong trạng thái miễn sát thương hoàn toàn.");
    }

    public void TakeDamage(int damage)
    {
        if (invincibleOnTrigger)
        {
            Debug.Log("Miễn sát thương do trigger đặc biệt!");
            return;
        }
        currentHealth -= damage;
        if (currentHealth <= 1 && isImmortal)
        {
            Debug.Log("bắn bố mày đi tao có khiêng.");
            currentHealth = 1;
            return;
        }

        if (currentHealth <= 0)
            currentHealth = 0;

    }
    public void OnMove()
    {
        if (isImmortal)
        {
            moveCount++;

            if (moveCount >= maxMoveCount)
            {
                isImmortal = false;
                Debug.Log("hết khiêng r.");
            }
        }
        if (invincibleOnTrigger)
        {
            invincibleMoveCount++;
            if (invincibleMoveCount >= maxInvincibleMoveCount)
            {
                invincibleOnTrigger = false;
                Debug.Log("Hết miễn sát thương khi chạm vật thể.");
            }
        }
    }

}
