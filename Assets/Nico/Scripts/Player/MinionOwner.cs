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
        minions.AddRange(FindObjectsByType<PlayerMinion>(FindObjectsSortMode.None));

        OnMinionsUpdated?.Invoke(minions);
    }
    private void AddMinion(PlayerMinion newMinion)
    {
        minions.Add(newMinion);
        OnMinionsUpdated.Invoke(minions);
    }
    private void RemoveMinion(PlayerMinion deadMinion)
    {
        minions.Remove(deadMinion);
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
        //Debug.Log($"{MinionSelected.name} moves bak ");
        MinionSelected.ReturnToPlayer();
    }
    public void ChangeTarget(Enemy target)
    {
        //Debug.Log($"{MinionSelected.name} attacks {target}");
        MinionSelected.AttackTarget(target.gameObject);
    }
    public void MoveToPosition(Vector3 posToMove)
    {
        //Debug.Log($"{MinionSelected.name} moves to the position {posToMove}");
        MinionSelected.MoveToPosition(posToMove);
    }
    #endregion
}
