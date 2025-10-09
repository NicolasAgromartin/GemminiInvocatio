using System;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(SphereCollider))]
public class EnemyDetector : MonoBehaviour
{
    public static event Action<GameObject> OnTargetChanged;
    public static bool EnemyInRange { get; private set; } = false;
    public GameObject SelectedTarget { get; private set; } = null;
    private readonly List<GameObject> enemiesNearby = new();
    private readonly float detectionRadius = 10f;
    private int lastIndexedTarget = 0;
    private GameObject detected;


    #region Life Cycle
    private void Awake()
    {
        GetComponent<SphereCollider>().radius = detectionRadius;
    }
    private void OnEnable()
    {
        InputManager.OnSwitchTargetButtonPressed += ChangeFocusedTarget;
    }
    private void OnDisable()
    {
        InputManager.OnSwitchTargetButtonPressed -= ChangeFocusedTarget;
    }
    #endregion



    #region Collision Trigger
    private void OnTriggerEnter(Collider other)
    {
        detected = other.transform.root.gameObject;

        if (!detected.CompareTag("Enemy") || enemiesNearby.Contains(detected)) return; 

        enemiesNearby.Add(detected);

        EnemyInRange = true;

        if(SelectedTarget == null)
        {
            SelectedTarget = detected;
            OnTargetChanged?.Invoke(SelectedTarget);
        }

        detected.GetComponent<Unit>().OnDeath += RemoveFromList;
    }
    private void OnTriggerExit(Collider other)
    {
        detected = other.transform.root.gameObject;

        if (!detected.CompareTag("Enemy") || !enemiesNearby.Contains(detected)) return;

        enemiesNearby.Remove(detected);

        if(enemiesNearby.Count == 0)
        {
            EnemyInRange = false;
            SelectedTarget = null;
        }
        else if (detected == SelectedTarget)
        {
            SelectedTarget = enemiesNearby[0];
        }

        detected.GetComponent<Unit>().OnDeath -= RemoveFromList;

        OnTargetChanged?.Invoke(SelectedTarget);
    }
    #endregion







    private void ChangeFocusedTarget()
    {
        if (enemiesNearby.Count == 0)
        {
            OnTargetChanged?.Invoke(null);
            return;
        }

        foreach(GameObject enemy in enemiesNearby) // pasarlo a un while
        {
            if(enemy == null || !enemy.GetComponent<Enemy>()) enemiesNearby.Remove(enemy);
        }


        lastIndexedTarget++;

        if (lastIndexedTarget >= enemiesNearby.Count)
        {
            lastIndexedTarget = 0;
        }

        SelectedTarget = enemiesNearby[lastIndexedTarget];
        OnTargetChanged?.Invoke(SelectedTarget);
    }
    private void RemoveFromList(Unit enemy)
    {
        enemy.GetComponent<Unit>().OnDeath -= RemoveFromList;
        enemiesNearby.Remove(enemy.gameObject);
    }








}
