using System.Runtime.CompilerServices;
using UnityEngine;



public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private float zOffset = 4f;
    [SerializeField] private float yOffset = -1f;
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





    #region Life Cykle
    private void Start()
    {
        CursorManager.DisableCursor();
    }
    private void OnEnable()
    {
        InputManager.OnLookAction += RotateCamera;
    }
    private void OnDisable()
    {
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


    public void EnterTacticalMode()
    {
        tacticalViewEnable = true;

        Camera.main.orthographic = true;

        transform.position = new Vector3(tacicalViewPosition.x + target.position.x, 
            tacicalViewPosition.y, tacicalViewPosition.z + target.position.z);
        transform.rotation = Quaternion.Euler(tacticalViewRotation);

        InputManager.OnLookAction -= RotateCamera;

    }
    public void ExitTacticalMode()
    {
        tacticalViewEnable = false;

        Camera.main.orthographic = false;

        InputManager.OnLookAction += RotateCamera;


    }
}
