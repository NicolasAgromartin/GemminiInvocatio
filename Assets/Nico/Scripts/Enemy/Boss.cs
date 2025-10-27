using System.Collections.Generic;
using UnityEngine;



public class Boss : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private GameObject spawnerPrefab;
    
    private int lives = 3;


    private List<DarkCrystal> crystals;
    private List<EnemySpawner> spawners;


    private void Awake()
    {
        crystals.AddRange(FindObjectsByType<DarkCrystal>(FindObjectsSortMode.None));
    }
    private void Start()
    {
        
    }
}
