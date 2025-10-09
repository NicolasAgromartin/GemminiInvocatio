using System;
using UnityEngine;



public class PlayerInteractState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerInteractState(PlayerStateMachine stateMachine) : base(stateMachine) { }







    public override void EnterState()
    { 
        animator.SetFloat("Movement", 0);
        CheckPossibleInteractions();
        
        NecromancyScreen.OnButtonPressed_CloseMenu += EndInteraction;
        CursorManager.EnableCursor();
    }
    public override void ExitState() 
    {
        NecromancyScreen.OnButtonPressed_CloseMenu -= EndInteraction;
        CursorManager.DisableCursor();
    }
    public override void UpdateState() 
    { 
    }




    private void CheckPossibleInteractions()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Interactable"));

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                collider.gameObject.transform.root.transform.gameObject.GetComponent<IInteractable>()?.Interact(transform.gameObject);
                //collider.gameObject.transform.root.transform.gameObject.GetComponent<IInteractable>()?.Interact(stateMachine.gameObject);
            }
        }
    }
    private void EndInteraction()
    {
        OnEventOccurred?.Invoke(TransitionEvent.End);
    }







}

