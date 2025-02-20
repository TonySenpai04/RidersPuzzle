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

    public void TakeDamage(int damage)
    {
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
    }
}
