using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Stats Stats { get; protected set; }

    public virtual void RecieveDamage(int damage) => Stats.Health -= damage;
    public virtual void IncreaseHealth(int amount) => Stats.Health += amount;
    public virtual int GetCurrentHealth() => Stats.Health;
}