using System;
using UnityEngine;

public class PlayerInteractState : BaseState
{
    public override event Action<PlayerEvent> OnEventOccurred;

    public PlayerInteractState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
        animator = stateMachine.Animator;
    }

    private PlayerStateMachine stateMachine;
    private Animator animator;
    private readonly float sphereRadius = 1f;

    public override void EnterState()
    {
        stateMachine.GetComponentInChildren<RemainsCanvas>(true).OnRemainsCanvasClosed += EndInteraction;

        animator.SetFloat("Movement", 0);
        CheckPossibleInteractions();
    }
    public override void ExitState() 
    {
        stateMachine.GetComponentInChildren<RemainsCanvas>(true).OnRemainsCanvasClosed -= EndInteraction;
    }
    public override void UpdateState() { }




    private void CheckPossibleInteractions()
    {
        Collider[] colliders = Physics.OverlapSphere(stateMachine.transform.position, sphereRadius);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                collider.gameObject.GetComponent<IInteractable>()?.Interact(stateMachine.gameObject);
            }
        }
    }

    private void EndInteraction()
    {
        OnEventOccurred?.Invoke(PlayerEvent.End);
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

