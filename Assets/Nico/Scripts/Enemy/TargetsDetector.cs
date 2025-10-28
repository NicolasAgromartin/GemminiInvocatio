using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class TargetsDetector : MonoBehaviour
{
    public event Action<GameObject> OnTargetsUpdated;

    public GameObject SelectedTarget;
    [SerializeField] private List<GameObject> targetsList = new();
    private GameObject detected;



    private void OnTriggerEnter(Collider other)
    {
        detected = other.gameObject;

        if (!detected.CompareTag("Player") && !detected.CompareTag("PlayerMinion")) return;

        if (targetsList.Contains(detected)) return;

        targetsList.Add(detected);
        detected.GetComponent<Unit>().OnDeath += RemoveMissingTarget;

        if(SelectedTarget == null)
        {
            SelectedTarget = detected;
            OnTargetsUpdated?.Invoke(SelectedTarget);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        detected = other.gameObject;

        if (!detected.CompareTag("Player") && !detected.CompareTag("PlayerMinion")) return;

        if (targetsList.Contains(detected))
        {
            targetsList.Remove(detected);
            if(detected == SelectedTarget) SelectedTarget = null;
            OnTargetsUpdated?.Invoke(SelectedTarget);

            detected.GetComponent<Unit>().OnDeath -= RemoveMissingTarget;
        }


    }



    private void RemoveMissingTarget(Unit target)
    {
        targetsList.Remove(target.gameObject);

        if(target.gameObject == SelectedTarget)
        {
            SelectedTarget = targetsList.Count > 0 ? targetsList.First() : null;
        }

        OnTargetsUpdated?.Invoke(SelectedTarget);
    }



    public void EnforceTarget(Unit target)
    {
        SelectedTarget = target.gameObject;
        targetsList.Add(SelectedTarget);
        OnTargetsUpdated?.Invoke(SelectedTarget);
    }
}
