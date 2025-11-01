using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;





public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject enemyPrefab;
    [Header("Enemies")]
    [SerializeField] private FiendSO zombieData;
    [SerializeField] private FiendSO skeletonData;

    [SerializeField] private float timeInterval;




    private void Start()
    {
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        while (enabled)
        {
            FiendSO selectedData = Random.value > 0.5f ? zombieData : skeletonData;

            enemyPrefab.SetActive(false);
            GameObject enemyObj = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            Fiend fiend = enemyObj.GetComponent<Fiend>();
            fiend.SetFiendData(selectedData);

            yield return new WaitForSeconds(timeInterval);

            enemyObj.SetActive(true);
            enemyObj.GetComponentInChildren<TargetsDetector>().EnforceTarget(GetTarget());
        }
    }


    // agregar un metodo forceTarget en el targetsDetector
    // randomizar entre todas las unidades que sean player o minion
    // asignar random de las unidades instanciadas que las persigan


    private Unit GetTarget()
    {
        List<GameObject> targets = new();

        foreach(PlayerMinion minion in FindObjectsByType<PlayerMinion>(FindObjectsSortMode.None))
        {
            targets.Add(minion.gameObject);
        }

        targets.Add(FindAnyObjectByType<Player>().gameObject);

        if (targets.Count == 0) return null;

        int randomIndex = Random.Range(0, targets.Count);
        return targets[randomIndex].GetComponent<Unit>();
    }
    // que pasa si es null el target?
}




// contar cuantos enemigos spawneo
// si tiene menos dle limite sigo spawneando
// suscribirse al evento de muerte del enemigo
// si llega al limite espera a que muera algun enemigo
// e instancia otro