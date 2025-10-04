using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class Player : Unit
{
    #region Events
    public event Action<List<PlayerMinion>> OnMinionsUpdated;
    public event Action<int> OnHealthChanged;
    public event Action<int> OnLivesChanged;
    public event Action OnDamageRecieved;
    public event Action OnLifeLost;
    public event Action OnPlayerLost;
    #endregion



    #region Components
    private Necromancy necromancy;
    private Inventory inventory;
    private List<PlayerMinion> minions = new();
    private RespawnManager respawnManager;
    #endregion



    #region Life Cykle
    private void Awake()
    {
        necromancy = GetComponent<Necromancy>();
        Stats = SaveSystem.LoadPlayerUnitStats();
        InitiateInventory();
    }
    private void Start()
    {
        OnHealthChanged?.Invoke(Stats.CurrentHealth);
        OnLivesChanged?.Invoke(lives);
        UpdateMinionsList();
    }
    private void OnEnable()
    {
        necromancy.OnUnitSummoned += AddMinion;
        necromancy.OnUnitResurrected += AddMinion;
        necromancy.OnUnitDefleshed += AddMinion;

        RespawnManager.OnPlayerRespawned += RestorePlayer;
    }
    private void OnDisable()
    {
        necromancy.OnUnitSummoned -= AddMinion;
        necromancy.OnUnitResurrected -= AddMinion;
        necromancy.OnUnitDefleshed -= AddMinion;

        RespawnManager.OnPlayerRespawned -= RestorePlayer;
    }
    #endregion



    #region Health & Life
    private int lives = 3;
    override public void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);

        OnDamageRecieved?.Invoke();
        OnHealthChanged?.Invoke(Stats.CurrentHealth);

        if (Stats.CurrentHealth <= 0)
        {
            tag = "Untagged";
            lives--;

            if (lives <= 0)
            {
                Debug.Log("Player died");
                // fin de juego
                OnPlayerLost?.Invoke();
                return;    
            }

            OnLifeLost?.Invoke();
            OnLivesChanged?.Invoke(lives);
        }
    }
    override public void IncreaseHealth(int health)
    {
        base.IncreaseHealth(health);
        OnHealthChanged?.Invoke(Stats.CurrentHealth);
    }

    private void RestorePlayer()
    {
        tag = "Player";
        Stats.CurrentHealth = 0;
        IncreaseHealth(80);
    }
    #endregion





    #region Minions
    private void UpdateMinionsList()
    {
        minions = FindObjectsByType<PlayerMinion>(sortMode: FindObjectsSortMode.None).ToList();
        OnMinionsUpdated?.Invoke(minions);
    }
    private void AddMinion(GameObject newMinion)
    {
        minions.Add(newMinion.GetComponent<PlayerMinion>());
        OnMinionsUpdated.Invoke(minions);
    }
    private void RemoveMinion(GameObject deadMinion)
    {
        minions.Remove(deadMinion.GetComponent<PlayerMinion>());
        OnMinionsUpdated?.Invoke(minions);
    }
    #endregion






    #region Inventory
    public void InitiateInventory()
    {
        inventory = new();
        FindAnyObjectByType<InventoryCanvas>(FindObjectsInactive.Include).SetInventory(inventory);
        GetComponent<PlayerStateMachine>().SetInventory(inventory);
        necromancy.SetInventory(inventory);
    }
    public Inventory GetInventory() => this.inventory;
    public Item UseKey()
    {
        return inventory.GetKey();
    }
    #endregion



}
