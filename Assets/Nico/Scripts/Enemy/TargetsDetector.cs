using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class TargetsDetector : MonoBehaviour
{
    public event Action<GameObject> OnTargetsUpdated;

    [SerializeField] private List<GameObject> targetsList = new();
    [SerializeField] private GameObject selectedTarget;
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
                SuscribeToTarget(root);
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
            UnsuscribeToTarget(root);
            targetsList.Remove(root);
            OnTargetsUpdated?.Invoke(SelectTarget());
        }
    }



    //private void SuscribeToTarget(GameObject target)
    //{
    //    PlayerMinion playerMinion = target.GetComponent<PlayerMinion>();
    //    Player player = target.GetComponent<Player>();

    //    if (playerMinion != null)
    //    {
    //        playerMinion.OnDeath += RemoveMissingTarget;
    //    }
    //}
    //private void UnsuscribeToTarget(GameObject target)
    //{
    //    PlayerMinion playerMinion = target.GetComponent<PlayerMinion>();
    //    Player player = target.GetComponent<Player>();

    //    if (playerMinion != null)
    //    {
    //        playerMinion.OnDeath -= RemoveMissingTarget;
    //    }
    //    if(player != null)
    //    {
    //        player.OnDeath -= RemoveMissingTarget;
    //    }
    //}
    private void SuscribeToTarget(GameObject target)
    {
        Unit unit = target.GetComponent<Unit>();

        if (unit != null) unit.OnDeath += RemoveMissingTarget;
    }
    private void UnsuscribeToTarget(GameObject target)
    {
        Unit unit = target.GetComponent<Unit>();

        if (unit != null) unit.OnDeath -= RemoveMissingTarget;

    }
    private void RemoveMissingTarget(Unit target)
    {
        targetsList.Remove(target.gameObject);
        OnTargetsUpdated?.Invoke(SelectTarget());
    }





    /* proximamente aca va la logica para seleccionar al enemigo */
    private GameObject SelectTarget()
    {

        if (targetsList.Count > 0)
        {
            //targetIndicator.text = targetsList.First().name;
            selectedTarget = targetsList.First();
            return selectedTarget;
        }
        else
        {
            //targetIndicator.text = "no target";
            selectedTarget = null;
            return null; 
        }
    }
}
