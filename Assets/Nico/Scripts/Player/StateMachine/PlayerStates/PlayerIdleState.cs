using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }




    #region Life Cykle
    public override void EnterState()
    {
        InputManager.OnInteractAction += Interact;
        InputManager.OnPlayerMovement += MovePlayer;
        InputManager.OnBasicAttackPerformed += Attack;
        InputManager.OnUsePotionButtonPressed += UsePotion;
        InputManager.OnTacticalButtonPressed += EnterTacticalMode;
    }
    public override void ExitState()
    {
        InputManager.OnInteractAction -= Interact;
        InputManager.OnPlayerMovement -= MovePlayer;
        InputManager.OnBasicAttackPerformed -= Attack;
        InputManager.OnUsePotionButtonPressed -= UsePotion;
        InputManager.OnTacticalButtonPressed -= EnterTacticalMode;

    }
    public override void UpdateState() 
    {
        ApplyGravity();
    }
    public override void OnTriggerEnter(Collider other)
    {
        GameObject parent = other.transform.root.gameObject;

        if (other.CompareTag("DamageDealer"))
        {
            if(parent.CompareTag("Enemy")) OnEventOccurred?.Invoke(TransitionEvent.RecieveDamage);
        }
    }
    #endregion




    private void MovePlayer(Vector2 direction)
    {
        animator.SetFloat("Movement", new Vector3(direction.x, 0, direction.y).magnitude, .2f, Time.deltaTime);

        if (direction.x != 0 || direction.y != 0)
        {
            OnEventOccurred?.Invoke(TransitionEvent.Move);
        }   
    }

    private void Attack() => OnEventOccurred?.Invoke(TransitionEvent.Attack);
    private void EnterTacticalMode() => OnEventOccurred?.Invoke(TransitionEvent.Tactics);



    private float velocity;
    private readonly float defaultGravity = -9.807f;
    private readonly float gravityMultiplier = 3f;
    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            velocity = -1f;
        }
        else
        {
            velocity += defaultGravity * gravityMultiplier * Time.deltaTime;
            characterController.Move(Vector3.up * velocity * Time.deltaTime);
        }
    }

    new private void Interact()
    {
        base.Interact();

        if (detected.CompareTag("Remains"))
        {
            OnEventOccurred?.Invoke(TransitionEvent.Interact);
        }
    }
}
