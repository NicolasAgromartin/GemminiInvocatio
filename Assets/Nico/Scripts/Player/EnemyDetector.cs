using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[RequireComponent(typeof(SphereCollider))]
public class EnemyDetector : MonoBehaviour
{
    public static event Action<GameObject> OnTargetChanged;



    public GameObject SelectedTarget { get; private set; } = null;
    [SerializeField] private List<GameObject> enemiesNearby = new();
    private GameObject detected;






    #region Life Cycle
    private void OnEnable()
    {
        InputManager.OnSwitchTargetButtonPressed += ChangeFocusedTarget;
    }
    private void OnDisable()
    {
        InputManager.OnSwitchTargetButtonPressed -= ChangeFocusedTarget;

        SelectedTarget = null;
        enemiesNearby.Clear();
    }
    #endregion







    #region Collision Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        
        detected = other.gameObject;
            
        if (enemiesNearby.Contains(detected)) return; 

        // lo agrego a la lista y me suscribo a su muerte
        enemiesNearby.Add(detected);
        detected.GetComponent<Unit>().OnDeath += RemoveFromList;


        if(SelectedTarget == null)
        {
            SelectedTarget = detected;
            OnTargetChanged?.Invoke(SelectedTarget);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        detected = other.gameObject;   

        if (!enemiesNearby.Contains(detected)) return;

        // lo elimino de la lista y me desuscribo de su evento de muerte
        enemiesNearby.Remove(detected);
        detected.GetComponent<Unit>().OnDeath -= RemoveFromList;

        // si me quede sin enemigos en la lista selected target == null si no cambio al siguiente en la lista
        if(enemiesNearby.Count == 0)
        {
            SelectedTarget = null;
        }
        else if (detected == SelectedTarget)
        {
            SelectedTarget = enemiesNearby[0];
        }

        OnTargetChanged?.Invoke(SelectedTarget);
    }
    #endregion


    private void RemoveFromList(Unit enemy)
    {
        enemy.GetComponent<Unit>().OnDeath -= RemoveFromList;
        enemiesNearby.Remove(enemy.gameObject);
        SelectedTarget = null;
        OnTargetChanged?.Invoke(SelectedTarget);
    }




    private void ChangeFocusedTarget()
    {
        if (enemiesNearby.Count == 0) return;

        // Si no hay objetivo actual, seleccionamos el primero
        if (SelectedTarget == null)
        {
            SelectedTarget = enemiesNearby[0];
        }
        else
        {
            // Buscamos el índice del objetivo actual y pasamos al siguiente (cíclico)
            int currentIndex = enemiesNearby.IndexOf(SelectedTarget);
            currentIndex = (currentIndex + 1) % enemiesNearby.Count;
            SelectedTarget = enemiesNearby[currentIndex];
        }

        // Desmarcar todos los enemigos
        foreach (GameObject enemy in enemiesNearby)
        {
            if (enemy == null) continue;
            enemy.GetComponent<Enemy>().DismarkEnemy();
        }

        // Marcar el nuevo objetivo
        if (SelectedTarget != null)
        {
            SelectedTarget.GetComponent<Enemy>().MarkEnemy();
            OnTargetChanged?.Invoke(SelectedTarget);
        }
    }







}
