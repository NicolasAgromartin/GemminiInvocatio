using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Boss : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private GameObject spawnerPrefab;
    
    private int lives = 3;
    private UnitAudio audio;


    private List<DarkCrystal> crystals = new();
    private List<EnemySpawner> spawners = new();
    [SerializeField] private List<Transform> spawnerPositions;
    private readonly Dictionary<Transform, GameObject> spawnerInPostions = new();
    private int spawnCount = 0;



    private void Awake()
    {
        audio = GetComponent<UnitAudio>();
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
    private void OnDisable()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach(Enemy enemy in enemies)
        {
            Destroy(enemy);
        }
    }
    private void Start()
    {
        InstantiateSpawners();
        //GetComponent<UnitAudio>().PlayPatrollingSounds();
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
            audio.PlayDeathSound();
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
        Dissolver[] dissolvers = GetComponentsInChildren<Dissolver>();
        List<Coroutine> coroutines = new();

        // Iniciar todas las corrutinas al mismo tiempo
        foreach (Dissolver dissolver in dissolvers)
        {
            coroutines.Add(StartCoroutine(dissolver.Dissapear()));
        }

        // Esperar hasta que todas hayan terminado
        foreach (Coroutine c in coroutines)
        {
            yield return c;
        }

        Destroy(gameObject);
    }
}
