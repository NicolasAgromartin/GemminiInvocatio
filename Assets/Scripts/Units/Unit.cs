using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public UnitStats Stats { get; protected set; }

    public virtual void RecieveDamage(int damage) => Stats.Health -= damage;
    protected virtual void IncreaseHealth(int amount) => Stats.Health += amount;
}

[System.Serializable]
public class UnitStats
{
    public int Health;
    public int Attack;
    public int Defense;
    public float MaxMovementSpeed;
    public float AttackRange;

    public UnitStats(int health, int attack, int defense, float maxMovementSpeed, float attackRange)
    {
        Health = health;
        Attack = attack;
        Defense = defense;
        MaxMovementSpeed = maxMovementSpeed;
        AttackRange = attackRange;
    }

    public UnitStats(UnitStats stats)
    {
        Health = stats.Health;
        Attack = stats.Attack;
        Defense = stats.Defense;
        MaxMovementSpeed = stats.MaxMovementSpeed;
        AttackRange = stats.AttackRange;
    }
}


