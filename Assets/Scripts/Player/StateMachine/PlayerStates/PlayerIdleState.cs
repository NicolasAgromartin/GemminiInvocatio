using System;
using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public override event Action<PlayerEvent> OnEventOccurred;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
        animator = stateMachine.Animator;
        characterController = stateMachine.CharacterController;
        enemyDetector = stateMachine.EnemyDetector;
    }

    #region Components
    private Animator animator;
    private PlayerStateMachine stateMachine;
    private CharacterController characterController;
    private EnemyDetector enemyDetector;
    #endregion


    #region Life Cykle
    public override void EnterState()
    {
        InputManager.OnPlayerMovement += MovePlayer;
        InputManager.OnInteractAction += Interact;
        InputManager.OnBasicAttackPerformed += Attack;
        InputManager.OnTacticalButtonPressed += EnterTacticalMode;
        InputManager.OnSwitchTargetButtonPressed += enemyDetector.ChangeFocusedTarget;
        InputManager.OnReturnAllMinonsButtonPressed += TacticsSystem.ReturnAllMinions;
    }
    public override void ExitState()
    {
        InputManager.OnPlayerMovement -= MovePlayer;
        InputManager.OnInteractAction -= Interact;
        InputManager.OnBasicAttackPerformed -= Attack;
        InputManager.OnTacticalButtonPressed -= EnterTacticalMode;
        InputManager.OnSwitchTargetButtonPressed -= enemyDetector.ChangeFocusedTarget;
        InputManager.OnReturnAllMinonsButtonPressed -= TacticsSystem.ReturnAllMinions;
    }
    public override void UpdateState() 
    {
    }
    #endregion




    private void MovePlayer(Vector2 direction)
    {
        animator.SetFloat("Movement", new Vector3(direction.x, 0, direction.y).magnitude, .2f, Time.deltaTime);

        if(direction.x != 0 || direction.y != 0)
        {
            OnEventOccurred?.Invoke(PlayerEvent.Move);
        }
    }
    private void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(stateMachine.transform.position, 1f);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.GetComponent<IInteractable>() != null)
                {
                    OnEventOccurred?.Invoke(PlayerEvent.Interact);
                }
            }
        }
    }
    private void Attack() => OnEventOccurred?.Invoke(PlayerEvent.Attack);
    private void EnterTacticalMode() => OnEventOccurred?.Invoke(PlayerEvent.Tactics);



    #region Collisions && Triggers
    public override void OnCollisionEnter(Collider other) { }
    public override void OnCollisionExit(Collider other) { }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    #endregion
}
