using System;
using UnityEngine;

public class PlayerDeadState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;


    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }




    public override void EnterState() 
    {
        Time.timeScale = 1f;
        animator.SetBool("Death", true);
        RespawnManager.OnPlayerRespawned += HandleRespawn;
        //BlackScreen.OnBlackScreenVisible += HandleRespawn;
    }

    public override void ExitState() 
    {
        RespawnManager.OnPlayerRespawned -= HandleRespawn;
        //BlackScreen.OnBlackScreenVisible -= HandleRespawn;
    }

    public override void UpdateState() { }




    private void HandleRespawn()
    {
        animator.SetBool("Death", false);
        animator.Play("Movement", 0, 0f);

        OnEventOccurred?.Invoke(TransitionEvent.Respawn);
    }
}
