using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Boss : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private GameObject spawnerPrefab;
    
    private int lives = 3;


    private List<DarkCrystal> crystals = new();
    private List<EnemySpawner> spawners = new();
    [SerializeField] private List<Transform> spawnerPositions;
    private readonly Dictionary<Transform, GameObject> spawnerInPostions = new();
    private int spawnCount = 0;



    private void Awake()
    {
        crystals.AddRange(FindObjectsByType<DarkCrystal>(FindObjectsSortMode.None));

        foreach(Transform position in spawnerPositions)
        {
            spawnerInPostions.Add(position, null);
        }
    }
    private void OnEnable()
    {
        foreach(DarkCrystal crystal in crystals)
        {
            crystal.OnDestroy += RecieveDamage;
        }
    }
    private void Start()
    {
        InstantiateSpawners();
    }




    private void RecieveDamage(DarkCrystal crystal)
    {
        crystal.OnDestroy -= RecieveDamage;
        crystals.Remove(crystal);
        lives--;

        Debug.Log(lives);

        if(lives > 0)
        {
            // spawn new spawner
            InstantiateSpawners();

        }
        else
        {
            StartCoroutine(RunDestructionSequence());
        }
    }


    private void InstantiateSpawners()
    {

    }
    private IEnumerator RunDestructionSequence()
    {
        yield return null;
    }
}
