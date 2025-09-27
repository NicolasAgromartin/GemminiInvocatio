using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    #region States
    public BaseState PrevState { get; private set; }
    protected BaseState CurrentState;
    #endregion

    [Header("Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform model;

    protected readonly Dictionary<(BaseState, TransitionEvent), BaseState> eventMap = new();


    protected internal void ChangeState(BaseState nextState)
    {
        if(CurrentState == nextState) return;

        PrevState = CurrentState;
        CurrentState.ExitState();
        CurrentState = nextState;
        CurrentState.EnterState();
    }
}
