using System;
using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

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
            OnEventOccurred?.Invoke(TransitionEvent.Move);
        }   
    }
    private void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(stateMachine.transform.position, 1f, LayerMask.GetMask("Interactable"));

        // unicamente cuando es una interaccion de necromancia cambio de estado, si no unicamente tomo el objeto
        //Debug.Log(colliders.Length);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.root.gameObject.GetComponent<IInteractable>() != null)
            {
                if(collider.transform.root.gameObject.CompareTag("Remains")) OnEventOccurred?.Invoke(TransitionEvent.Interact);
                else
                {
                    collider.transform.root.gameObject.GetComponent<Pickable>().Interact(stateMachine.gameObject);
                }
            }
        }
    }
    private void Attack() => OnEventOccurred?.Invoke(TransitionEvent.Attack);
    private void EnterTacticalMode() => OnEventOccurred?.Invoke(TransitionEvent.Tactics);



    #region Collisions && Triggers
    public override void OnCollisionEnter(Collider other) { }
    public override void OnCollisionExit(Collider other) { }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    #endregion
}
