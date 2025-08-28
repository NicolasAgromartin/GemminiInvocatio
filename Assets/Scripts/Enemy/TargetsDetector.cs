using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TargetsDetector : MonoBehaviour
{
    public event Action<GameObject> OnTargetsUpdated;

    [SerializeField] private GameObject selectedTarget;
    [SerializeField] private List<GameObject> targetsList = new();
    [SerializeField] private float detectionRadius;

    [SerializeField] private TMP_Text targetIndicator;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerMinion"))
        {
            if(!targetsList.Contains(other.gameObject))
            {
                targetsList.Add(other.gameObject);
                OnTargetsUpdated?.Invoke(SelectTarget());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(targetsList.Contains(other.gameObject)) 
        {
            targetsList.Remove(other.gameObject);
            OnTargetsUpdated?.Invoke(SelectTarget());
        }
    }


    /* proximamente aca va la logica para seleccionar al enemigo */
    private GameObject SelectTarget()
    {

        if (targetsList.Count > 0)
        {
            targetIndicator.text = targetsList.First().name;
            selectedTarget = targetsList.First();
            return selectedTarget;
        }
        else
        {
            targetIndicator.text = "no target";
            selectedTarget = null;
            return null; 
        }
    }
}
