using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public virtual event Action<Unit> OnDeath;
    public event Action<Unit, int> OnDamageRecieved;
    public event Action<Unit, int> OnHealthIncreased;

    public Stats Stats { get; protected set; }


    public virtual void RecieveDamage(int damage)
    {
        Stats.CurrentHealth -= damage;

        if(Stats.CurrentHealth < 0) Stats.CurrentHealth = 0;

        OnDamageRecieved?.Invoke(this, Stats.CurrentHealth);

        if(Stats.CurrentHealth <= 0)
        {
            OnDeath?.Invoke(this);
            return;
        }

    }
    public virtual void IncreaseHealth(int amount)
    {
        Stats.CurrentHealth += amount;

        if(Stats.CurrentHealth > Stats.MaxHealth) Stats.CurrentHealth = Stats.MaxHealth;

        OnHealthIncreased?.Invoke(this, Stats.CurrentHealth);
    }
}