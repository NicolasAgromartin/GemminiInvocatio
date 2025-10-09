using System;
using UnityEngine;



public class PlayerMovementState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerMovementState(PlayerStateMachine stateMachine) : base(stateMachine) { }


    private readonly float modelRotationSpeed = 500f;
    private readonly float moveSpeed = 5f;

    private Quaternion modelRotation;
    private Vector3 moveDirection;
    private Vector3 playerVelocity;







    #region Life Cykle
    public override void EnterState()
    {
        InputManager.OnPlayerMovement += SimpleMove;
        InputManager.OnBasicAttackPerformed += Attack;
        InputManager.OnUsePotionButtonPressed += UsePotion;
        InputManager.OnInteractAction += Interact;

    }
    public override void ExitState()
    {
        InputManager.OnPlayerMovement -= SimpleMove;
        InputManager.OnBasicAttackPerformed -= Attack;
        InputManager.OnUsePotionButtonPressed -= UsePotion;
        InputManager.OnInteractAction -= Interact;

    }
    public override void UpdateState() 
    {
        ApplyGravity();
    }
    #endregion




    private void SimpleMove(Vector2 inputDirection)
    {
        animator.SetFloat("Movement", new Vector3(inputDirection.x, 0f, inputDirection.y).magnitude, .2f, Time.deltaTime);

        if (inputDirection.x == 0 && inputDirection.y == 0) OnEventOccurred?.Invoke(TransitionEvent.End);

        moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
        
        playerVelocity = CinemachineController.PlanarRotation * moveDirection * moveSpeed;


        if (Mathf.Clamp01(Mathf.Abs(inputDirection.x) + Mathf.Abs(inputDirection.y)) > 0)
        {
            modelRotation = Quaternion.LookRotation(playerVelocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, modelRotation, modelRotationSpeed * Time.deltaTime);
        }


        playerVelocity.y = velocity;
        characterController.Move(Time.deltaTime * playerVelocity);
    }


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
        }
    }


    private void Attack()
    {
        OnEventOccurred?.Invoke(TransitionEvent.Attack);
    }


}
