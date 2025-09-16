using System.Collections.Generic;
using UnityEngine;




public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesNearby = new();
    //[SerializeField] private float enemyDetectionRadius = 10f;

    private int lastIndexedTarget = 0;
    private GameObject root;






    private void OnTriggerEnter(Collider other)
    {
        root = other.transform.root.gameObject;

        if (!root.CompareTag("Enemy") || enemiesNearby.Contains(root)) return; 
        // si el gameObject no tiene el tag enemigo o si ya esta en la lista

        enemiesNearby.Add(root);
        root.GetComponent<Enemy>().OnDeath += RemoveFromList;
    }
    private void OnTriggerExit(Collider other)
    {
        root = other.transform.root.gameObject;

        if (!root.CompareTag("Enemy") || !enemiesNearby.Contains(root)) return;

        enemiesNearby.Remove(root);
        root.GetComponent<Enemy>().OnDeath -= RemoveFromList;

        if (TacticsSystem.SelectedEnemy == root.gameObject) TacticsSystem.UnselectEnemy();
    }


    public void ChangeFocusedTarget()
    {
        if (enemiesNearby.Count == 0) return;

        foreach(GameObject enemy in enemiesNearby) // pasarlo a un while
        {
            if(enemy == null || !enemy.GetComponent<Enemy>()) enemiesNearby.Remove(enemy);
        }


        lastIndexedTarget++;

        if (lastIndexedTarget >= enemiesNearby.Count)
        {
            lastIndexedTarget = 0;
        }

        TacticsSystem.SelectEnemy(enemiesNearby[lastIndexedTarget]);
    }



    private void RemoveFromList(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().OnDeath -= RemoveFromList;
        enemiesNearby.Remove(enemy.gameObject);
        TacticsSystem.UnselectEnemy();
    }
}
