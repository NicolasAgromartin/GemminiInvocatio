using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class InputManager : Singleton<InputManager>
{
    #region Events
    public static event Action<Vector2> OnPlayerMovement;
    public static event Action<Vector2> OnLookAction;
    public static event Action OnBasicAttackPerformed;
    public static event Action OnInteractAction;
    public static event Action OnTacticalButtonPressed;
    public static event Action OnSwitchTargetButtonPressed;
    public static event Action OnReturnAllMinonsButtonPressed;
    public static event Action OnSwarmTargetButtonPressed;

    public static event Action<GameObject> OnPlayerMinionSelected;
    public static event Action<Vector3> OnPositionSelected;
    public static event Action<GameObject> OnEnemySelected;
    #endregion

    private PlayerInput playerInput;

    #region PlayerActions
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction basicAttackAction;
    private InputAction interactAction;
    private InputAction tacticalAction;
    private InputAction switchTargetAction;
    private InputAction returnAllMinionsAction;
    private InputAction swarmTargetAction;
    private InputAction leftClickAction;
    #endregion






    #region Life Cykle
    new private void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        basicAttackAction = playerInput.actions.FindAction("BasicAttack");
        interactAction = playerInput.actions.FindAction("Interact");
        tacticalAction = playerInput.actions.FindAction("Tactical");
        switchTargetAction = playerInput.actions.FindAction("SwitchTarget");
        returnAllMinionsAction = playerInput.actions.FindAction("ReturnAllMinions");
        swarmTargetAction = playerInput.actions.FindAction("SwarmTarget");
        leftClickAction = playerInput.actions.FindAction("LeftClick");
    }
    private void OnEnable()
    {
        basicAttackAction.performed += BasicAttackAction;
        interactAction.performed += InteractAction;
        tacticalAction.performed += TacticalAction;
        lookAction.performed += LookAction;
        switchTargetAction.performed += SwitchTargetAction;
        returnAllMinionsAction.performed += ReturnAllMinionsAction;
        swarmTargetAction.performed += SwarmTargetAction;
        leftClickAction.performed += LeftClickAction;
    }
    private void OnDisable()
    {
        basicAttackAction.performed -= BasicAttackAction;
        interactAction.performed -= InteractAction;
        tacticalAction.performed -= TacticalAction;
        lookAction.performed -= LookAction;
        switchTargetAction.performed -= SwitchTargetAction;
        returnAllMinionsAction.performed -= ReturnAllMinionsAction;
        swarmTargetAction.performed -= SwarmTargetAction;
        leftClickAction.performed -= LeftClickAction;
    }
    private void Update()
    {
        OnPlayerMovement?.Invoke(moveAction.ReadValue<Vector2>().normalized);
    }
    #endregion








    private void LookAction(InputAction.CallbackContext context)
    {
        OnLookAction?.Invoke(context.ReadValue<Vector2>().normalized);
    }
    private void BasicAttackAction(InputAction.CallbackContext context)
    {
        OnBasicAttackPerformed?.Invoke();
    }
    private void InteractAction(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke();
    }
    private void TacticalAction(InputAction.CallbackContext context)
    {
        OnTacticalButtonPressed?.Invoke();
    }
    private void SwitchTargetAction(InputAction.CallbackContext context)
    {
        OnSwitchTargetButtonPressed?.Invoke();
    }
    private void ReturnAllMinionsAction(InputAction.CallbackContext context)
    {
        OnReturnAllMinonsButtonPressed?.Invoke();
    }
    private void SwarmTargetAction(InputAction.CallbackContext context)
    {
        OnSwarmTargetButtonPressed?.Invoke();
    }

    private void LeftClickAction(InputAction.CallbackContext context)
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.transform.root.gameObject.CompareTag("Enemy"))
            {
                OnEnemySelected?.Invoke(hit.collider.gameObject);
            }
            if (hit.collider.transform.root.gameObject.CompareTag("PlayerMinion"))
            {
                OnPlayerMinionSelected?.Invoke(hit.collider.gameObject);
            }
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                OnPositionSelected?.Invoke(hit.point);
            }
        }
    }
}
// el rigid body y el collider para deteectar al enemigo esta en el hijo no en el parent lo que dificulta la
// deteccion del enemigo con un click




