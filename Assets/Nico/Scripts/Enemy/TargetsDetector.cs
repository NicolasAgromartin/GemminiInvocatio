using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class TargetsDetector : MonoBehaviour
{
    public event Action<GameObject> OnTargetsUpdated;

    public GameObject SelectedTarget;
    [SerializeField] private List<GameObject> targetsList = new();
    private GameObject root;






    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DetectionCollider")) return;
        
        root = other.transform.root.gameObject;


        if (root.CompareTag("Player") || root.CompareTag("PlayerMinion"))
        {
            if (!targetsList.Contains(root))
            {
                targetsList.Add(root);
                SuscribeToTarget(root.GetComponent<Unit>());
                OnTargetsUpdated?.Invoke(SelectTarget());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("DetectionCollider")) return;

        root = other.transform.root.gameObject;

        if (targetsList.Contains(root)) 
        {
            targetsList.Remove(root);
            UnsuscribeToTarget(root.GetComponent<Unit>());
            SelectTarget();
            OnTargetsUpdated?.Invoke(SelectedTarget);
        }
    }



    private void SuscribeToTarget(Unit target)
    {
        if (target != null) target.OnDeath += RemoveMissingTarget;
    }
    private void UnsuscribeToTarget(Unit target)
    {
        if (target != null) target.OnDeath -= RemoveMissingTarget;
    }
    private void RemoveMissingTarget(Unit target)
    {
        targetsList.Remove(target.gameObject);
        OnTargetsUpdated?.Invoke(SelectTarget());
    }





    /* proximamente aca va la logica para seleccionar al enemigo */
    private GameObject SelectTarget()
    {
        foreach(GameObject target in targetsList)
        {
            if (target == null) targetsList.Remove(target);
        }

        if (targetsList.Count > 0)
        {
            SelectedTarget = targetsList.First();
            return SelectedTarget;
        }
        else
        {
            SelectedTarget = null;
            return null; 
        }
    }
}
