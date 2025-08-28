using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class Player : Unit
{
    public event Action<GameObject> OnMinionDead;
    public event Action<List<PlayerMinion>> OnMinionsUpdated;

    public event Action<int> OnHealthChanged;
    public event Action<int> OnLivesChanged;

    public event Action OnDamageRecieved;
    public event Action OnLifeLost;


    [SerializeField] private List<PlayerMinion> minions = new();







    #region Life Cykle
    private void Awake()
    {
        Stats = SaveSystem.LoadPlayerUnitStats();
    }
    private void OnEnable()
    {
        NecromancySystem.OnUnitResurrected += AddMinion;
    }
    private void OnDisable()
    {
        NecromancySystem.OnUnitResurrected -= AddMinion;
    }
    private void Start()
    {
        OnHealthChanged?.Invoke(Stats.Health);
        OnLivesChanged?.Invoke(lives);
        UpdateMinionsList();
    }
    #endregion



    #region Health & Life
    private int lives = 3;
    override public void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);

        OnDamageRecieved?.Invoke();
        OnHealthChanged?.Invoke(Stats.Health);

        if (Stats.Health <= 0)
        {
            lives--;
            OnLifeLost?.Invoke();
            OnLivesChanged?.Invoke(lives);
            Stats.Health = 5;
            // matar al player, respawnear en zona de respawn cercana a cada area

            if (lives <= 0)
            {
                // fin de juego
            }
        }
    }
    #endregion


    #region Minions
    private void UpdateMinionsList()
    {
        minions = FindObjectsByType<PlayerMinion>(sortMode: FindObjectsSortMode.None).ToList();
        OnMinionsUpdated?.Invoke(minions);
    }
    private void AddMinion(GameObject newMinion) => minions.Add(newMinion.GetComponent<PlayerMinion>());
    private void RemoveMinion(GameObject deadMinion)
    {
        OnMinionDead?.Invoke(deadMinion);
        minions.Remove(deadMinion.GetComponent<PlayerMinion>());
    }
    #endregion

}


