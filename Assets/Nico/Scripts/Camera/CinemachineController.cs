using Unity.Cinemachine;
using UnityEngine;





public class CinemachineController : MonoBehaviour
{
    public static Quaternion PlanarRotation {  get; private set; }

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineCamera interactionCamera;
    [SerializeField] private CinemachineCamera tacticalCamera;
    [SerializeField] private CinemachineCamera combatCamera;
    private CinemachineBasicMultiChannelPerlin cameraNoise;

    [Header("Target Group")]
    [SerializeField] private CinemachineTargetGroup targetGroup;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeAmplitude;

    private Transform player;
    private Transform enemy;




    private void Awake()
    {
        player = FindAnyObjectByType<Player>().transform;

        mainCamera.Follow = player;
        interactionCamera.Follow = player;
        tacticalCamera.Follow = player;
        combatCamera.Follow = player;

        cameraNoise = combatCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void OnEnable()
    {
        PlayerStateMachine.OnStateChange += HandlePlayerStateChange;

        //EnemyDetector.OnTargetChanged += HandleEnemyDetection;
        //Weapon.OnEnemyHitted += ShakeCamera;
    }
    private void OnDisable()
    {
        PlayerStateMachine.OnStateChange -= HandlePlayerStateChange;

        //EnemyDetector.OnTargetChanged -= HandleEnemyDetection;
        //Weapon.OnEnemyHitted -= ShakeCamera;
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


    private void HandleEnemyDetection(GameObject enemyDected)
    {
        if(enemyDected == null)
        {
            combatCamera.gameObject.SetActive(false);
            targetGroup.RemoveMember(enemyDected.transform);

            mainCamera.gameObject.SetActive(true);
        }
        else
        {
            combatCamera.gameObject.SetActive(true);
            targetGroup.AddMember(enemyDected.transform, 0f, 0f);

            mainCamera.gameObject.SetActive(false);
        }
    }
}
