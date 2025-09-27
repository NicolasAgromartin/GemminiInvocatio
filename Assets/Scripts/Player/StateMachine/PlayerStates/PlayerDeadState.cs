using System;
using UnityEngine;

public class PlayerDeadState : BaseState
{
    public PlayerDeadState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override event Action<TransitionEvent> OnEventOccurred;



    public override void EnterState() { }

    public override void ExitState() { OnEventOccurred?.Invoke(TransitionEvent.End); }

    public override void UpdateState() { }





    public override void OnCollisionEnter(Collider other) { }
    public override void OnCollisionExit(Collider other) { }

    public override void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new NotImplementedException();
    }
}
