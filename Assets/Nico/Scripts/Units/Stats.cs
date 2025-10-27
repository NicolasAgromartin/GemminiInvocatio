


[System.Serializable]
public class Stats
{
    public int MaxHealth;
    public int CurrentHealth;
    public int Attack;


    public Stats(int health, int attack, float maxMovementSpeed, float attackRange)
    {
        MaxHealth = health;
        CurrentHealth = MaxHealth;
        Attack = attack;
    }

    public Stats(Stats stats)
    {
        MaxHealth = stats.MaxHealth;
        CurrentHealth = MaxHealth;
        Attack = stats.Attack;
    }
}


