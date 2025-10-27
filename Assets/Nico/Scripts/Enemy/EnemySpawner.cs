using UnityEngine;
using System.Collections.Generic;


public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject skeletonPrefab;

    private float timeInterval = 10f;
    private bool enabled = false;


}
