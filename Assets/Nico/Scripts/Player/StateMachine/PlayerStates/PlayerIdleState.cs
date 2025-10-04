using System;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    #region Components
    private Animator animator;
    private PlayerStateMachine stateMachine;
    private EnemyDetector enemyDetector;
    private AudioSource stepsClip;
    #endregion


    #region Life Cykle
    public override void EnterState()
    {
        animator = stateMachine.Animator;
        enemyDetector = stateMachine.EnemyDetector;
        stepsClip = stateMachine.StepsClip;

        InputManager.OnPlayerMovement += MovePlayer;
        InputManager.OnInteractAction += Interact;
        InputManager.OnBasicAttackPerformed += Attack;
        InputManager.OnTacticalButtonPressed += EnterTacticalMode;
        InputManager.OnSwitchTargetButtonPressed += enemyDetector.ChangeFocusedTarget;
        InputManager.OnReturnAllMinonsButtonPressed += TacticsSystem.ReturnAllMinions;

        stepsClip.Stop();
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


        Debug.Log(colliders.Length);
        // unicamente cuando es una interaccion de necromancia cambio de estado, si no unicamente tomo el objeto
        //Debug.Log(colliders.Length);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.transform.root.gameObject;
            Debug.Log(collider.gameObject.name);

            if (obj.GetComponent<IInteractable>() != null)
            {

                if(obj.CompareTag("Remains")) OnEventOccurred?.Invoke(TransitionEvent.Interact);
                else 
                {
                    obj.GetComponent<IInteractable>().Interact(stateMachine.gameObject);
                }
            }

            if(collider.gameObject.GetComponent<IInteractable>() != null)
            {
                if (collider.gameObject.CompareTag("Gate"))
                {
                    Debug.Log("Gate detected");
                }

                collider.gameObject.GetComponent<IInteractable>().Interact(stateMachine.gameObject);
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
