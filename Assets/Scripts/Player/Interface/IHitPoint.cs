public interface IHitPoint
{
    float GetMaxHealth();
    float GetCurrentHealth();
    void SetHealth(float value);
    void TakeDamage(int damage);


    void Heal(int amount);

}