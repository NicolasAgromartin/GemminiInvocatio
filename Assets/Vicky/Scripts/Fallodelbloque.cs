using UnityEngine;
using System.Collections;

public class Fallodelbloque : MonoBehaviour
{
    private Rigidbody body;
    private Vector3 initPos;
    private Quaternion initRot;

    void Start()
    {
        
        body = GetComponent<Rigidbody>();
        initPos = transform.position;
        initRot = transform.rotation;

        
        body.useGravity = false;
        body.isKinematic = true;
    }

    void OnTriggerEnter(Collider obj)
    {
        
        if (obj.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        
        yield return new WaitForSeconds(0.25f);

        body.isKinematic = false;
        body.useGravity = true;

        StartCoroutine(Return());
    }

    IEnumerator Return()
    {
        yield return new WaitForSeconds(5f);

        body.useGravity = false;
        body.isKinematic = true;
        body.linearVelocity = Vector3.zero;
    }
}