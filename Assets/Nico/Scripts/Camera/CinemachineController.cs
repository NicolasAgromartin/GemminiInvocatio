using Unity.Cinemachine;
using UnityEngine;
using System;




public class CinemachineController : MonoBehaviour
{
    public static event Action OnTitleScreenCameraPositioned;

    public Quaternion PlanarRotation { get; private set; }

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineCamera interactionCamera;
    [SerializeField] private CinemachineCamera tacticalCamera;
    [SerializeField] private CinemachineCamera combatCamera;
    [SerializeField] private CinemachineCamera titleScreenCamera;


    private Transform player;




    private void Awake()
    {
        player = FindAnyObjectByType<Player>().transform;

        mainCamera.Follow = player;
        interactionCamera.Follow = player;
        tacticalCamera.Follow = player;
        combatCamera.Follow = player;
    }
    private void OnEnable()
    {
        PlayerStateMachine.OnStateChange += HandlePlayerStateChange;
        
    }
    private void OnDisable()
    {
        PlayerStateMachine.OnStateChange -= HandlePlayerStateChange;


    }
    private void Update()
    {
        PlanarRotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
    }







    private void HandlePlayerStateChange(BaseState currentState)
    {
        switch (currentState)
        {
            case PlayerTacticsState:
                tacticalCamera.gameObject.SetActive(true);

                mainCamera.gameObject.SetActive(false);
                interactionCamera.gameObject.SetActive(false);
                break;
            case PlayerInteractState:
                interactionCamera.gameObject.SetActive(true);

                mainCamera.gameObject.SetActive(false);
                tacticalCamera.gameObject.SetActive(false);
                break;
            case PlayerDeadState:
                mainCamera.gameObject.SetActive(true);
                tacticalCamera.gameObject.SetActive(false);
                interactionCamera.gameObject.SetActive(false);
                break;
            default:
                mainCamera.gameObject.SetActive(true);

                interactionCamera.gameObject.SetActive(false);
                tacticalCamera.gameObject.SetActive(false);
                break;
        }
    }




    #region TitleScreen
    private void DeactivateTitleScreenCamera()
    {
        titleScreenCamera.gameObject.SetActive(false);

        mainCamera.gameObject.SetActive(true);
        interactionCamera.gameObject.SetActive(false);
        tacticalCamera.gameObject.SetActive(false);
    }
    private void ActivateTitleScreenCamera()
    {
        titleScreenCamera.gameObject.SetActive(true);

        mainCamera.gameObject.SetActive(false);
        interactionCamera.gameObject.SetActive(false);
        tacticalCamera.gameObject.SetActive(false);
    }
    #endregion

}
