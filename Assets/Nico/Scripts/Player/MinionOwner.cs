using System;
using System.Collections.Generic;
using UnityEngine;





public class MinionOwner:MonoBehaviour
{
    public static event Action<PlayerMinion> OnMinionSelected;
    public static PlayerMinion MinionSelected { get; private set; }
    public static event Action<List<PlayerMinion>> OnMinionsUpdated;

    private readonly List<PlayerMinion> minions = new();
    private EnemyDetector enemyDetector;





    private void Awake()
    {
        enemyDetector = GetComponentInChildren<EnemyDetector>();
    }
    private void OnEnable()
    {
        InputManager.OnSwarmTargetButtonPressed += SwarmTarget;
        InputManager.OnReturnAllMinonsButtonPressed += CallAllMinions;

        Necromancy.OnNewMinionCreated += AddMinion;
    }
    private void OnDisable()
    {
        InputManager.OnSwarmTargetButtonPressed -= SwarmTarget;
        InputManager.OnReturnAllMinonsButtonPressed -= CallAllMinions;

        Necromancy.OnNewMinionCreated -= AddMinion;
    }
    private void Start()
    {
        UpdateMinionsList();
    }





    #region Minions Managment
    public void UpdateMinionsList()
    {
        PlayerMinion[] detectedMinions = FindObjectsByType<PlayerMinion>(FindObjectsSortMode.None);

        foreach(PlayerMinion minion in detectedMinions)
        {
            minions.Add(minion);
            minion.OnDeath += RemoveMinion;
        }

        OnMinionsUpdated?.Invoke(minions);
    }
    private void AddMinion(PlayerMinion newMinion)
    {
        minions.Add(newMinion);
        newMinion.OnDeath += RemoveMinion;
        OnMinionsUpdated.Invoke(minions);
    }
    private void RemoveMinion(Unit deadMinion)
    {
        deadMinion.OnDeath -= RemoveMinion;
        minions.Remove(deadMinion.GetComponent<PlayerMinion>());
        OnMinionsUpdated?.Invoke(minions);
    }
    #endregion 








    public void SetMinionSelected(PlayerMinion minionSelected)
    {
        MinionSelected = minionSelected;
        OnMinionSelected?.Invoke(MinionSelected);
    }




    #region Global Orders
    private void CallAllMinions()
    {
        foreach(PlayerMinion minion in minions)
        {
            minion.ReturnToPlayer();
        }
    }
    private void SwarmTarget()
    {
        if (enemyDetector.SelectedTarget == null) return;

        foreach(PlayerMinion minion in minions)
        {
            minion.AttackTarget(enemyDetector.SelectedTarget);
        }
    }
    #endregion




    #region Single Orders
    public void ReturnToPlayer()
    {
        MinionSelected.ReturnToPlayer();
    }
    public void ChangeTarget(Enemy target)
    {
        MinionSelected.AttackTarget(target.gameObject);
    }
    public void MoveToPosition(Vector3 posToMove)
    {
        Debug.Log("Order recieved");
        MinionSelected.MoveToPosition(posToMove);
    }
    #endregion
}
