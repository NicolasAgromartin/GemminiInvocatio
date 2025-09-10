


[System.Serializable]
public class Stats
{
    public int Health;
    public int Attack;
    public float MaxMovementSpeed;
    public float AttackRange;

    public Stats(int health, int attack, float maxMovementSpeed, float attackRange)
    {
        Health = health;
        Attack = attack;
        MaxMovementSpeed = maxMovementSpeed;
        AttackRange = attackRange;
    }

    public Stats(Stats stats)
    {
        Health = stats.Health;
        Attack = stats.Attack;
        MaxMovementSpeed = stats.MaxMovementSpeed;
        AttackRange = stats.AttackRange;
    }
}


