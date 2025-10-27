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
        if (!other.CompareTag("DamageDealer")) return;

        Weapon weapon = other.GetComponent<Weapon>();

        if(weapon && weapon.IsEnemy) OnEventOccurred?.Invoke(TransitionEvent.RecieveDamage);
    }
    #endregion



    private float movementValue;
    private void MovePlayer(Vector2 direction)
    {
        movementValue = new Vector3(direction.x, 0f, direction.y).magnitude;

        //animator.SetFloat("Movement", movementValue, .2f, Time.deltaTime);
        //animator.SetFloat("Movement", 0f);
        if (animator.GetFloat("Movement") > 0.1 || movementValue > 0)
        {
            animator.SetFloat("Movement", movementValue, .2f, Time.deltaTime);
        }
        else if (movementValue == 0)
        {
            animator.SetFloat("Movement", 0);
        }

        if (movementValue > 0.05f)
            OnEventOccurred?.Invoke(TransitionEvent.Move);
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

        if (detected != null && detected.CompareTag("Remains"))
        {
            OnEventOccurred?.Invoke(TransitionEvent.Interact);
        }
    }
}
