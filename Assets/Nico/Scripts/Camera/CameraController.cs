using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;



public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private float zOffset = 4f;
    [SerializeField] private float yOffset = -1f;
    private readonly float zInteractionOffset = 2f;
    private readonly float yInteractionOffset = -1.4f;
    private readonly float zBaseOffset = 4f;
    private readonly float yBaseOffset = -1f;
    private Quaternion targetRotation;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float minVerticalAngle = -15;
    [SerializeField] private float maxVerticalAngle = 45;
    private float yRotation;
    private float xRotation;

    [Header("Frame")]
    [SerializeField] private Vector2 characterFrame;
    private Vector3 framingOffset;

    [Header("Tactical View")]
    [SerializeField] private bool tacticalViewEnable;

    private readonly Vector3 tacicalViewPosition = new(-5f, 5f, -1f);
    private readonly Vector3 tacticalViewRotation = new(30f, 45f, 0f);

    private RaycastHit[] walls;
    private Player player;


    #region Life Cykle
    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void Start()
    {
        CursorManager.DisableCursor();
    }
    private void OnEnable()
    {
        PauseManager.OnPauseToggled += PauseCamera;
        InputManager.OnLookAction += RotateCamera;


    }
    private void OnDisable()
    {
        PauseManager.OnPauseToggled += PauseCamera;
        InputManager.OnLookAction -= RotateCamera;

    }
    private void LateUpdate()
    {
        if (tacticalViewEnable) return;
        
        targetRotation = Quaternion.Euler(xRotation, yRotation, 0);
        framingOffset = target.position + new Vector3(characterFrame.x, characterFrame.y);

        transform.position = framingOffset - targetRotation * new Vector3(0f, yOffset, zOffset);
        transform.rotation = targetRotation;
    }
    #endregion








    private void RotateCamera(Vector2 lookDirection)
    {
        xRotation += lookDirection.y * rotationSpeed;
        yRotation += lookDirection.x * rotationSpeed;

        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
    }
    public Quaternion PlanarRotation() => Quaternion.Euler(0f, yRotation, 0);





    #region Tactical Mode
    public void EnterTacticalMode()
    {
        tacticalViewEnable = true;

        //Camera.main.orthographic = true;
        //Camera.main.orthographicSize = 8;
        //Camera.main.nearClipPlane = -10;

        HideWalls();

        transform.position = new Vector3(
            tacicalViewPosition.x + transform.position.x, 
            //tacicalViewPosition.x + target.position.x,
            tacicalViewPosition.y, 
            //tacicalViewPosition.z + target.position.z);
            tacicalViewPosition.z + transform.position.z);

        //transform.rotation = Quaternion.Euler(
        //    new(tacticalViewRotation.x, 
        //    transform.rotation.y + tacticalViewRotation.y, 
        //    tacticalViewRotation.z) );

        InputManager.OnLookAction -= RotateCamera;

    }
    public void ExitTacticalMode()
    {
        tacticalViewEnable = false;

        ShowWalls();

        //Camera.main.orthographic = false;
        //Camera.main.nearClipPlane = .01f;

        InputManager.OnLookAction += RotateCamera;
    }
    private void HideWalls()
    {
        walls = Physics.BoxCastAll(
            (target.position - transform.position).normalized,
            new Vector3(Camera.main.orthographicSize / 2, Camera.main.orthographicSize / 2, Camera.main.orthographicSize / 2),
            target.transform.position, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("Wall"));

        foreach (RaycastHit wall in walls)
        {
            wall.collider.gameObject.SetActive(false);
        }
    }
    private void ShowWalls()
    {
        foreach (RaycastHit wall in walls)
        {
            wall.collider.gameObject.SetActive(true);
        }
    }
    #endregion 





    #region Interaction


    public void ZoomToInteract()
    {
        zOffset = zInteractionOffset;
        yOffset = yInteractionOffset;
        InputManager.OnLookAction -= RotateCamera;
    }
    public void EndInteractionZoom()
    {
        zOffset = zBaseOffset;
        yOffset = yBaseOffset;
        InputManager.OnLookAction += RotateCamera;
    }
    #endregion





    private void PauseCamera(bool isGamePaused)
    {
        if (isGamePaused)
        {
            InputManager.OnLookAction -= RotateCamera;
        }
        else
        {
            InputManager.OnLookAction += RotateCamera;
        }
    }
}
