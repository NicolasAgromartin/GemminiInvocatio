using System.Collections.Generic;
using UnityEngine;





public class RespawnManager : Singleton<RespawnManager>
{
    [Header("Respawn Points")]
    [SerializeField] private List<GameObject> respawnPoints;
    private Player player;
    private GameObject closestRespawnPoint;



    new private void Awake()
    {
        base.Awake();
        player = FindAnyObjectByType<Player>();
    }
    private void OnEnable()
    {
        player.OnLifeLost += RespawnPlayer;
    }
    private void OnDisable()
    {
        player.OnLifeLost -= RespawnPlayer;
    }




    
    private void RespawnPlayer()
    {
        Debug.Log("Respawn");
        closestRespawnPoint = respawnPoints[0];

        foreach(GameObject respawnPoint in respawnPoints)
        {
            if(Vector3.Distance(respawnPoint.transform.position, player.transform.position) <=
                Vector3.Distance(closestRespawnPoint.transform.position, player.transform.position))
            {
                closestRespawnPoint = respawnPoint;
                player.transform.position = respawnPoint.transform.position;
            }
        }
    }
    
    // se suscribe al evento de muerte del jugador
    // calcula donde murio el jugador y lo reinstancia en  el punto
    // de respawn mas cercano

    // generar dinamicamente los puntos de respawn?
    // cuando muere el jugador se toma su posicion en el mundo
    // calcular una distancia desde donde murio ( como evito que esa distancia no lo haga avanzar )
    // uso un raycast para tomar una posicion valida
}
