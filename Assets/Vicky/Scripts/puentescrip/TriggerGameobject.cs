using UnityEngine;
[RequireComponent(typeof(Collider))]
public class PlanKTrigger : MonoBehaviour
{
    FallingPlank parentPlank;

    void Start()
    {
        parentPlank = GetComponentInParent<FallingPlank>();
        if (parentPlank == null)
            Debug.LogWarning("PlankTrigger: no se encontró FallingPlank en los padres.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (parentPlank != null)
            parentPlank.OnPlayerStepped(other.gameObject);
    }
}
