using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float rotationAmount = 90f;
    [SerializeField]
    private float forwardDirection = 0f;


    private Vector3 StartRotation;
    private Vector3 forward;
    private Coroutine coroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        forward = transform.right;

        
    }

    public void Open(Vector3 UserPosition)
    {
        Debug.Log("llamando a open");
        if (!isOpen)
        {

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            if (isRotatingDoor)
            {
                float dot = Vector3.Dot(forward, (UserPosition - transform.position).normalized);
                coroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }


    private IEnumerator DoRotationOpen(float FowardAmount)

    {
        Quaternion startRotation = transform.rotation;
        Vector3 euler = startRotation.eulerAngles;

        Quaternion endRotation;
        if (FowardAmount >= forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(euler.x, euler.y - rotationAmount, euler.z));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(euler.x, euler.y + rotationAmount, euler.z));
        }

        isOpen = true;
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        Debug.Log("llamando a close");
        if (isOpen)
        {
            if (coroutine != null) 
            {
                StopCoroutine(coroutine);
            }


            if (isRotatingDoor)
            {
                coroutine = StartCoroutine(DoRotationClose());
            }

        }

    }

    private IEnumerator DoRotationClose()
    {
          Quaternion startRotation=transform.rotation;
          Quaternion endRotation = Quaternion.Euler(StartRotation);

        isOpen = false; 
        float time = 0;
        while (time < 1) 
        {
            transform.rotation= Quaternion.Slerp(startRotation,endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

    }


}
