using System.Collections; 
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class FallingPlank : MonoBehaviour
{
    [Header("Timings")]
    [SerializeField] float delayBeforeFall = 0.5f;     
    [SerializeField] float destroyAfter = 0.5f;        

    [Header("Physics")]
    [SerializeField] float extraImpulse = 1.5f;      
    [SerializeField] bool unparentOnFall = true;

    Rigidbody rb;
    bool triggered = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
      
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    
    public void OnPlayerStepped(GameObject player)
    {
        if (triggered) return;
        if (!player.CompareTag("Player")) return; 
        triggered = true;
        StartCoroutine(FallRoutine());
    }

    IEnumerator FallRoutine()
    {
        
        yield return new WaitForSeconds(delayBeforeFall);

        
        if (unparentOnFall) transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        
        rb.AddTorque(Random.onUnitSphere * extraImpulse, ForceMode.Impulse);
        rb.AddForce(Vector3.down *20f , ForceMode.Impulse);

        Destroy(gameObject, destroyAfter);
    }
}
