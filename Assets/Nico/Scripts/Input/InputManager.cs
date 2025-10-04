using System;
using UnityEngine;
using UnityEngine.InputSystem;





public class InputManager : MonoBehaviour
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
    public static event Action OnUsePotionButtonPressed;
    public static event Action<GameObject> OnPlayerMinionSelected;
    public static event Action<Vector3> OnPositionSelected;
    public static event Action<GameObject> OnEnemySelected;
    public static event Action OnPauseButtonPressed;
    #endregion



    [SerializeField] private LayerMask layersToInteract;




    #region Player InputActions
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction basicAttackAction;
    private InputAction interactAction;
    private InputAction tacticalAction;
    private InputAction switchTargetAction;
    private InputAction returnAllMinionsAction;
    private InputAction swarmTargetAction;
    private InputAction usePotionAction;
    private InputAction leftClickAction;
    private InputAction pauseGameAction;
    #endregion





    #region Life Cykle
    private void Awake()
    {
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
        usePotionAction = playerInput.actions.FindAction("UsePotion");
        pauseGameAction = playerInput.actions.FindAction("PauseGame");

    }
    private void OnEnable()
    {
        SuscribeToInputActions();
        PauseManager.OnPauseToggled += HandlePause;
    }
    private void OnDisable()
    {
        UnsuscribeToInputActions();
        PauseManager.OnPauseToggled -= HandlePause;
    }
    private void Update()
    {
        OnPlayerMovement?.Invoke(moveAction.ReadValue<Vector2>().normalized);
    }
    #endregion







    #region Input Actions
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
    private void UsePotionAction(InputAction.CallbackContext context)
    {
        OnUsePotionButtonPressed?.Invoke();
    }
    private void LeftClickAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), Mathf.Infinity, layersToInteract);
        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.transform.root.gameObject.CompareTag("Enemy"))
            {
                OnEnemySelected?.Invoke(hit.collider.transform.root.gameObject);
            }
            if (hit.collider.transform.root.gameObject.CompareTag("PlayerMinion"))
            {
                Debug.Log(hit.collider.gameObject);
                OnPlayerMinionSelected?.Invoke(hit.collider.transform.root.gameObject);
            }
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                OnPositionSelected?.Invoke(hit.point);
            }
        }
    }
    private void PauseGameAction(InputAction.CallbackContext context)
    {
        OnPauseButtonPressed?.Invoke();
    }
    #endregion


    private void SuscribeToInputActions()
    {
        basicAttackAction.performed += BasicAttackAction;
        interactAction.performed += InteractAction;
        tacticalAction.performed += TacticalAction;
        lookAction.performed += LookAction;
        switchTargetAction.performed += SwitchTargetAction;
        returnAllMinionsAction.performed += ReturnAllMinionsAction;
        swarmTargetAction.performed += SwarmTargetAction;
        leftClickAction.performed += LeftClickAction;
        usePotionAction.performed += UsePotionAction;
        pauseGameAction.performed += PauseGameAction;
    }
    private void UnsuscribeToInputActions()
    {
        basicAttackAction.performed -= BasicAttackAction;
        interactAction.performed -= InteractAction;
        tacticalAction.performed -= TacticalAction;
        lookAction.performed -= LookAction;
        switchTargetAction.performed -= SwitchTargetAction;
        returnAllMinionsAction.performed -= ReturnAllMinionsAction;
        swarmTargetAction.performed -= SwarmTargetAction;
        leftClickAction.performed -= LeftClickAction;
        usePotionAction.performed -= UsePotionAction;
        pauseGameAction.performed -= PauseGameAction;
    }
    private void HandlePause(bool isGamePaused)
    {
        if (isGamePaused)
        {
            UnsuscribeToInputActions();
        }
        else
        {
            SuscribeToInputActions();
        }
    }
}




