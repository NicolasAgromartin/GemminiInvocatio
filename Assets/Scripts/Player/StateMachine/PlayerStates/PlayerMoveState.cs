using System;
using UnityEngine;



public class PlayerMovementState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerMovementState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    #region Components
    private PlayerStateMachine stateMachine;
    private CharacterController characterController;
    private CameraController cameraController;
    private EnemyDetector enemyDetector;
    private AudioSource stepsClip;
    private Transform characterModel;
    private Animator animator;
    #endregion


    private readonly float defaultGravity = -9.807f;
    private readonly float gravityMultiplier = 3f;
    private readonly float modelRotationSpeed = 500f;
    private readonly float moveSpeed = 5f;

    private Quaternion modelRotation;

    private Vector3 moveDirection;
    private Vector3 moveInput;
    private Vector3 playerVelocity;
    private float moveAmount;
    private float velocity;






    #region Life Cykle
    public override void EnterState()
    {
        characterController = stateMachine.CharacterController;
        cameraController = stateMachine.CameraController;
        characterModel = stateMachine.CharacterModel;
        animator = stateMachine.Animator;
        enemyDetector = stateMachine.EnemyDetector;
        stepsClip = stateMachine.StepsClip;

        InputManager.OnPlayerMovement += MovePlayer;
        InputManager.OnSwitchTargetButtonPressed += enemyDetector.ChangeFocusedTarget;
        InputManager.OnReturnAllMinonsButtonPressed += TacticsSystem.ReturnAllMinions;
    }
    public override void ExitState()
    {
        InputManager.OnPlayerMovement -= MovePlayer;
        InputManager.OnSwitchTargetButtonPressed -= enemyDetector.ChangeFocusedTarget;
        InputManager.OnReturnAllMinonsButtonPressed -= TacticsSystem.ReturnAllMinions;
    }
    public override void UpdateState() 
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
        
        if (moveAmount > 0) // si el jugador se está moviendo
        {
            if (!stepsClip.isPlaying) // solo arranca si no está ya sonando
                stepsClip.Play();
        }
        else // si no hay movimiento
        {
            if (stepsClip.isPlaying)
                stepsClip.Stop();
        }

    }
    #endregion



    private void MovePlayer(Vector2 direction)
    {
        animator.SetFloat("Movement", new Vector3(direction.x, 0, direction.y).magnitude, .2f, Time.deltaTime);

        if (direction.x == 0 && direction.y == 0) OnEventOccurred?.Invoke(TransitionEvent.End);

        moveInput = new Vector3(direction.x, 0f, direction.y).normalized;
        moveAmount = Mathf.Clamp01(Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
        moveDirection = cameraController.PlanarRotation() * moveInput;

        playerVelocity = moveDirection * moveSpeed;
    }
    private void ApplyMovement()
    {
        characterController.Move(Time.deltaTime * playerVelocity);
    }
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

        playerVelocity.y = velocity;
    }
    private void ApplyRotation()
    {
        if (moveAmount > 0)
        {
            modelRotation = Quaternion.LookRotation(moveDirection);
            characterModel.rotation = Quaternion.RotateTowards(characterModel.transform.rotation, modelRotation, modelRotationSpeed * Time.deltaTime);
        }
    }



    #region Physics
    public override void OnCollisionEnter(Collider other) { }
    public override void OnCollisionExit(Collider other) { }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    #endregion
}
