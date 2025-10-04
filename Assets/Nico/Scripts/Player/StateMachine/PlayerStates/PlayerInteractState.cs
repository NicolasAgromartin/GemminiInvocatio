using System;
using UnityEngine;

public class PlayerInteractState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerInteractState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
        animator = stateMachine.Animator;
        remainsCanvas = stateMachine.RemainsCanvas;
        cameraController = stateMachine.CameraController;
        necromancy = stateMachine.Necromancy;
    }

    #region Components
    private readonly PlayerStateMachine stateMachine;
    private readonly NecromancyScreen remainsCanvas;
    private readonly Necromancy necromancy;
    private readonly CameraController cameraController;
    private readonly Animator animator;
    #endregion

    private readonly float sphereRadius = 1f;





    public override void EnterState()
    {
        remainsCanvas.OnRemainsCanvasClosed += EndInteraction;

        animator.SetFloat("Movement", 0);
        CheckPossibleInteractions();
    }
    public override void ExitState() 
    {
        remainsCanvas.OnRemainsCanvasClosed -= EndInteraction;
    }
    public override void UpdateState() { }




    private void CheckPossibleInteractions()
    {
        Collider[] colliders = Physics.OverlapSphere(stateMachine.transform.position, sphereRadius, LayerMask.GetMask("Interactable"));


        if (colliders.Length > 0)
        {
            cameraController.ZoomToInteract();

            foreach (Collider collider in colliders)
            {
                collider.gameObject.transform.root.transform.gameObject.GetComponent<IInteractable>()?.Interact(stateMachine.gameObject);
            }
        }
    }
    private void EndInteraction()
    {
        cameraController.EndInteractionZoom();
        OnEventOccurred?.Invoke(TransitionEvent.End);
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

