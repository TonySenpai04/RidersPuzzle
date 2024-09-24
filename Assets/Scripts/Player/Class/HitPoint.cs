using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : IHitPoint
{
    private float health;
    private float currentHealth;
    public HitPoint(float health)
    {
        this.health = health;
        this.currentHealth = health;
    }
    public void Heal(int amount)
    {
        this.currentHealth += amount;
        if (currentHealth > health)
            currentHealth = health;
    }
    public float GetHealth()
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
    }

    public void TakeDamage(int damage)
    {
        this.currentHealth -= damage;
        Debug.Log(currentHealth);
    }
}
