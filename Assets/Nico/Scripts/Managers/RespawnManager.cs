using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class RespawnManager : Singleton<RespawnManager>
{
    public static event Action OnPlayerRespawned;
    

    private Player player;
    private GameObject closestRespawnPoint;
    private readonly List<GameObject> respawnPoints = new();
    

    private void Start()
    {
        foreach (Transform point in transform)
        {
            respawnPoints.Add(point.gameObject);
        }
        player = FindAnyObjectByType<Player>(); 

    }



    public void RespawnPlayer()
    {
        closestRespawnPoint = respawnPoints[0];

        foreach (GameObject respawnPoint in respawnPoints)
        {
            float distCurrent = Vector3.Distance(respawnPoint.transform.position, player.transform.position);
            float distClosest = Vector3.Distance(closestRespawnPoint.transform.position, player.transform.position);

            if (distCurrent < distClosest)
            {
                closestRespawnPoint = respawnPoint;
            }
        }

        // recién acá lo movés
        player.transform.position = closestRespawnPoint.transform.position;

        OnPlayerRespawned?.Invoke();

        CursorManager.DisableCursor();
        Time.timeScale = 1f;
    }

    // se suscribe al evento de muerte del jugador
    // calcula donde murio el jugador y lo reinstancia en  el punto
    // de respawn mas cercano

    // generar dinamicamente los puntos de respawn?
    // cuando muere el jugador se toma su posicion en el mundo
    // calcular una distancia desde donde murio ( como evito que esa distancia no lo haga avanzar )
    // uso un raycast para tomar una posicion valida
}

