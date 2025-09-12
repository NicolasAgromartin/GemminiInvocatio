using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTacticsState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerTacticsState(PlayerStateMachine stateMachine) : base(stateMachine) 
    {
        playerCanvas = stateMachine.PlayerCanvas;
    }


    private PlayerCanvas playerCanvas;
    private GameObject playerMinionSelected;
    //private GameObject enemySelected;





    #region Life Cykle
    public override void EnterState()
    {
        EnableTacticalMode();
    }
    public override void UpdateState()
    {
        //throw new NotImplementedException();
    }
    public override void ExitState()
    {
        InputManager.OnTacticalButtonPressed -= DisableTacticalMode;

        InputManager.OnEnemySelected -= SelectEnemy;
        InputManager.OnPlayerMinionSelected -= SelectPlayerMinion;

        playerMinionSelected = null;
        //enemySelected = null;
        playerCanvas.HideMinionTactics();
    }
    #endregion







    // cuando abro el modal me suscribo a los eventos de los botones
    // cuando se ejecuta el evento del boton me desuscribo de los eventos
    #region PlayerMinion Selection
    private void SelectPlayerMinion(GameObject playerMinion)
    {
        playerMinionSelected = playerMinion;
        playerCanvas.ShowMinionTactics(playerMinion);

        playerCanvas.OnMoveOrder += EnablePositionSelection;
        playerCanvas.OnReturnOrder += ReturnOrder;
        playerCanvas.OnTargetChangeOrder += EnableEnemySelection;

        TacticsSystem.SelectPlayerMinion(playerMinion);
    }
    #endregion





    #region Position Selection
    private void EnablePositionSelection()
    {
        Debug.Log("Select position");
        InputManager.OnPositionSelected += SelectPosition;

        playerCanvas.OnMoveOrder -= EnablePositionSelection;
        playerCanvas.OnReturnOrder -= ReturnOrder;
        playerCanvas.OnTargetChangeOrder -= EnableEnemySelection;
    }
    private void SelectPosition(Vector3 position)
    {
        InputManager.OnPositionSelected -= SelectPosition;
        TacticsSystem.SelectPosition(position);
        TacticsSystem.UnselectMinion();
    }
    #endregion

    #region Enemy Selection
    private void EnableEnemySelection()
    {
        Debug.Log("Select enemy");
        InputManager.OnEnemySelected += SelectEnemy;

        playerCanvas.OnMoveOrder -= EnablePositionSelection;
        playerCanvas.OnReturnOrder -= ReturnOrder;
        playerCanvas.OnTargetChangeOrder -= EnableEnemySelection;
    }
    private void SelectEnemy(GameObject enemy)
    {

        playerCanvas.ChangeFocusedTarget(enemy);
        TacticsSystem.ChangeTarget(enemy);
    }
    #endregion

    #region Return Order
    private void ReturnOrder()
    {
        TacticsSystem.ReturnToPlayer();

        playerCanvas.OnMoveOrder -= EnablePositionSelection;
        playerCanvas.OnReturnOrder -= ReturnOrder;
        playerCanvas.OnTargetChangeOrder -= EnableEnemySelection;
    }
    #endregion





    // habilitar el modo tactico suscribe a la seleccion de minion
    private void EnableTacticalMode()
    {
        Time.timeScale = .05f;
        Camera.main.GetComponent<CameraController>().EnterTacticalMode();
        CursorManager.EnableCursor();

        InputManager.OnTacticalButtonPressed += DisableTacticalMode;
        InputManager.OnPlayerMinionSelected += SelectPlayerMinion;
    }
    private void DisableTacticalMode()
    {
        Time.timeScale = 1f;
        Camera.main.GetComponent<CameraController>().ExitTacticalMode();
        CursorManager.DisableCursor();

        OnEventOccurred?.Invoke(TransitionEvent.Tactics);
    }





    #region Triggers && Collisions
    public override void OnCollisionEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    public override void OnCollisionExit(Collider other)
    {
        throw new NotImplementedException();
    }

    public override void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new NotImplementedException();
    }
    #endregion
}
