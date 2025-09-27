using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class RespawnManager : Singleton<RespawnManager>
{
    public static event Action OnPlayerRespawned;
    

    [Header("Respawn Points")]
    [SerializeField] private List<GameObject> respawnPoints;
    private GameObject closestRespawnPoint;
    
    private Player player;


    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject deathScreen;



    #region Life Cykle
    new private void Awake()
    {
        base.Awake();
        player = FindAnyObjectByType<Player>();
    }
    private void OnEnable()
    {
        player.OnLifeLost += OpenDeathScreen;
    }
    private void OnDisable()
    {
        player.OnLifeLost -= OpenDeathScreen;
    }
    #endregion



    private void OpenDeathScreen()
    {
        blackScreen.SetActive(true);
        deathScreen.SetActive(true);

        CursorManager.EnableCursor();
        Time.timeScale = 0f;

        SetButtonsEvents();
    }
    
    private void SetButtonsEvents()
    {
        Button tryAgainButton = deathScreen.transform.Find("ButtonContainer/TryAgainButton").gameObject.GetComponent<Button>();
        Button mainMenuButton = deathScreen.transform.Find("ButtonContainer/MainMenuButton").gameObject.GetComponent<Button>();
        Button exitButton = deathScreen.transform.Find("ButtonContainer/ExitButton").gameObject.GetComponent<Button>();


        tryAgainButton.onClick.AddListener(() =>
        {
            RespawnPlayer();
            tryAgainButton.onClick.RemoveAllListeners();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.GoToTitleScreen();
            tryAgainButton.onClick.RemoveAllListeners();
        });

        exitButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.ExitGame();
            tryAgainButton.onClick.RemoveAllListeners();
        });
    }

    private void RespawnPlayer()
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

        blackScreen.SetActive(false);
        deathScreen.SetActive(false);

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

