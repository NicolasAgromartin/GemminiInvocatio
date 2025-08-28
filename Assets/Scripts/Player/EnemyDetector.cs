using System;
using System.Collections.Generic;
using UnityEngine;




public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesNearby = new();

    private int lastIndexedTarget = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy") || enemiesNearby.Contains(other.gameObject)) return;

        enemiesNearby.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy") || !enemiesNearby.Contains(other.gameObject)) return;

        enemiesNearby.Remove(other.gameObject);
        if (TacticsSystem.SelectedEnemy == other.gameObject) TacticsSystem.UnselectEnemy();
    }


    public void ChangeFocusedTarget()
    {
        if (enemiesNearby.Count == 0) return;

        lastIndexedTarget++;

        if (lastIndexedTarget >= enemiesNearby.Count)
        {
            lastIndexedTarget = 0;
        }

        TacticsSystem.SelectEnemy(enemiesNearby[lastIndexedTarget]);
    }
}
