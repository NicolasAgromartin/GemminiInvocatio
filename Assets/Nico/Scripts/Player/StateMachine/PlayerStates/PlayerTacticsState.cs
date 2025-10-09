using System;
using UnityEngine;
using UnityEngine.Rendering;




public class PlayerTacticsState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerTacticsState(PlayerStateMachine stateMachine) : base(stateMachine) { }





    #region Life Cykle
    public override void EnterState()
    {
        EnableTacticalMode();
    }
    public override void UpdateState()
    {
    }
    public override void ExitState()
    {
        InputManager.OnTacticalButtonPressed -= DisableTacticalMode;
        //InputManager.OnEnemySelected -= SelectEnemy;
        InputManager.OnPlayerMinionSelected -= SelectMinion;

        UnsuscribeToButtons();

    }
    #endregion






    private void EnableTacticalMode()
    {
        Time.timeScale = .05f;
        CursorManager.EnableCursor();

        InputManager.OnTacticalButtonPressed += DisableTacticalMode;
        InputManager.OnPlayerMinionSelected += SelectMinion;
    }
    private void DisableTacticalMode()
    {
        Time.timeScale = 1f;
        CursorManager.DisableCursor();

        OnEventOccurred?.Invoke(TransitionEvent.Tactics);
        UnselectMinion();
    }






    #region Minion Selection
    private void SelectMinion(GameObject minionSelected)
    {
        minionOwner.SetMinionSelected(minionSelected.transform.root.GetComponent<PlayerMinion>());
        SuscribeToButtons();
    }
    private void UnselectMinion()
    {
        minionOwner.SetMinionSelected(null);
    }
    #endregion



    private void SuscribeToButtons()
    {
        UnsuscribeToButtons();

        TacticalScreen.OnButtonPressed_ChangeTarget += EnableEnemySelection;
        TacticalScreen.OnButtonPressed_Move += EnablePositionSelection;
    }
    private void UnsuscribeToButtons()
    {
        TacticalScreen.OnButtonPressed_ChangeTarget -= EnableEnemySelection;
        TacticalScreen.OnButtonPressed_Move -= EnablePositionSelection;
    }



    private void EnableEnemySelection()
    {
        InputManager.OnEnemySelected += SelectEnemy;
    }
    private void EnablePositionSelection()
    {
        InputManager.OnPositionSelected += PositionSelected;
    }




    private void SelectEnemy(GameObject enemySelected)
    {
        Debug.Log(enemySelected);
        minionOwner.ChangeTarget(enemySelected.transform.root.gameObject.GetComponent<Enemy>()); 
        UnsuscribeToInput();
    }
    private void PositionSelected(Vector3 pos)
    {
        Debug.Log(pos);
        minionOwner.MoveToPosition(pos);
        UnsuscribeToInput();
    }
    private void UnsuscribeToInput()
    {
        InputManager.OnPositionSelected -= PositionSelected;
        InputManager.OnEnemySelected -= SelectEnemy;
    }
}
