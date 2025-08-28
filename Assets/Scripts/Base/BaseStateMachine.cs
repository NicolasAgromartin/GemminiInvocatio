using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    public BaseState PrevState { get; private set; }
    protected BaseState CurrentState;

    [Header("Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform model;




    protected internal void ChangeState(BaseState nextState)
    {
        if(CurrentState == nextState) return;

        PrevState = CurrentState;
        CurrentState.ExitState();
        CurrentState = nextState;
        CurrentState.EnterState();
    }
}
