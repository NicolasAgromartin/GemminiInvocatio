using System;
using UnityEngine;



public abstract class BaseState
{
    public abstract event Action<TransitionEvent> OnEventOccurred;

    public BaseState(BaseStateMachine stateMachine) { }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();

    public abstract void OnCollisionEnter(Collider other);
    public abstract void OnCollisionExit(Collider other);
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerExit(Collider other);

}



