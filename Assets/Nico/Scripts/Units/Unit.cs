using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IDamageable
{
    public virtual event Action<Unit> OnDeath;
    public virtual event Action<Unit, int> OnDamageRecieved;
    public event Action<Unit, int> OnHealthIncreased;


    public Stats Stats { get; protected set; }





    protected virtual void OnTriggerEnter(Collider other)
    {
        if (CompareTag("DetectionCollider")) return;
        if (!other.CompareTag("DamageDealer")) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Weapon")) return;

        Weapon weapon = other.GetComponent<Weapon>();

        // el enemigo creo que transiciona a su mismo estado y se rompe trabado en takeDamage



        switch (weapon.IsEnemy)
        {
            case true:
                if (gameObject.CompareTag("Player") || gameObject.CompareTag("PlayerMinion"))
                    RecieveDamage(other.GetComponent<Weapon>().Damage);
                break;

            case false:
                if (gameObject.CompareTag("Enemy"))
                {
                    RecieveDamage(other.GetComponent<Weapon>().Damage);
                    Debug.Log($"{gameObject} recieved an attack, current health {Stats.CurrentHealth}");
                }
                break;
        }
    }






    protected virtual void RecieveDamage(int damage)
    {
        //Debug.Log($"{damage} recieved from UNIT");

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