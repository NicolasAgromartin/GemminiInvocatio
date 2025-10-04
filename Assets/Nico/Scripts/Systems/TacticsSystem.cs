using System;
using UnityEngine;

public static class TacticsSystem
{
    #region Events
    public static event Action<GameObject> OnPlayerMinionSelected;
    public static event Action OnMinionUnselected;

    public static event Action<GameObject> OnEnemySelected;
    public static event Action OnEnemyUnselected;

    public static event Action<GameObject> OnSwarmOrder;
    public static event Action OnQuickReturn;


    public static event Action<Vector3> OnMoveToPositionOrder;
    public static event Action<GameObject> OnTargetChangeOrder;
    public static event Action OnReturnOrder;

    public static event Action OnPositionSelectionEnabled;
    public static event Action OnEnemySelectionEnabled;
    #endregion



    public static GameObject SelectedMinion { get; private set; }
    public static GameObject SelectedEnemy { get; private set; }



    #region Click Selections
    public static void SelectPlayerMinion(GameObject minionSelected)
    {
        SelectedMinion = minionSelected;
        OnPlayerMinionSelected?.Invoke(minionSelected);
    }
    public static void UnselectMinion()
    {
        SelectedMinion = null;
        OnMinionUnselected?.Invoke();
    }
    public static void SelectEnemy(GameObject enemySelected)
    {
        SelectedEnemy = enemySelected;
        //Debug.Log(enemySelected);
        OnEnemySelected?.Invoke(enemySelected);
    }
    public static void UnselectEnemy()
    {
        SelectedEnemy = null;
        OnEnemyUnselected?.Invoke();
    }
    public static void SelectPosition(Vector3 posToMove)
    {
        //Debug.Log(posToMove);
        OnMoveToPositionOrder?.Invoke(posToMove);
    }
    #endregion



    #region Global Actions
    public static void ReturnAllMinions() => OnQuickReturn?.Invoke();
    public static void SwarmEnemy(GameObject target)
    {
        if(SelectedEnemy == null) return;
        OnSwarmOrder?.Invoke(target);
    }
    #endregion



    #region UI Buttons
    public static void EnablePositionSelection()
    {
        Debug.Log("Now you must select a position to move");
        OnPositionSelectionEnabled?.Invoke();
        UnselectMinion();
    }
    public static void EnableTargetSelection()
    {
        Debug.Log("Now you must select an enemy to attack");
        OnEnemySelectionEnabled?.Invoke();
        UnselectMinion();
    }
    #endregion



    #region Particular Actions
    public static void MovePosition(Vector3 positionToMove) => OnMoveToPositionOrder?.Invoke(positionToMove);
    public static void ChangeTarget(GameObject newTarget)
    {
        Debug.Log(newTarget);   
        OnTargetChangeOrder?.Invoke(newTarget);
    }
    public static void ReturnToPlayer()
    {
        Debug.Log("Return order executed");
        OnReturnOrder?.Invoke();
    }
    #endregion
}
