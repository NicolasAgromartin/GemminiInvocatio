using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;




public class SceneLoader : Singleton<SceneLoader>
{
    public static event Action OnSceneLoaded;
    private Player player;
    private float loadRange;


    public enum SceneNames
    {
        TitleScreen,
        GameOverScreen,
        LoadingScreen,

        Core,

        Ottagono,
        Water,
        MainlandExterior,

        Hall,
        Library,
        Bedroom,
        Kitchen,
    }
    private readonly Dictionary<SceneNames, string> scenesByName = new()
    {
        { SceneNames.TitleScreen, "Nico_TitleScreen" },
        { SceneNames.Ottagono, "Nico_Ottagono" },
        { SceneNames.Water, "Nico_Water" },
        { SceneNames.Core, "Nico_Core"  },
    };
    private readonly Dictionary<int, SceneNames> scenesByIndex = new()
    {
        // el numero de index tiene que ser el mismo del build
        // ui
        { 0, SceneNames.TitleScreen },
        { 1, SceneNames.GameOverScreen },
        { 2, SceneNames.LoadingScreen },

        // player, camera y scripts de sistema
        { 3, SceneNames.Core },

        // exteriores
        { 4, SceneNames.Ottagono },
        { 5, SceneNames.Water },
        { 6, SceneNames.MainlandExterior},
        
        // interior del castillo
        { 7, SceneNames.Hall },
        { 8, SceneNames.Library },
        { 9, SceneNames.Bedroom },
        { 10, SceneNames.Kitchen },
    };
    private readonly Dictionary<SceneNames, bool> scenesLoading = new()
    {
        { SceneNames.Kitchen, false},
    };


    public SceneNames CurrentScene { get; private set; }
    private SceneNavigator sceneNavigator;
    private readonly List<AsyncOperation> scenesToLoad = new();




    #region Life Cycle
    new private void Awake()
    {
        base.Awake();
        gameObject.transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        SuscribeToNavigationEvents();
        OnSceneLoaded += SuscribeToNavigationEvents;
    }
    private void OnDestroy()
    {
        OnSceneLoaded -= SuscribeToNavigationEvents;
        UnsuscribeToNavigationEvents();
    }
    #endregion






    #region Navigation Events
    private void SuscribeToNavigationEvents()
    {
        sceneNavigator = FindAnyObjectByType<SceneNavigator>();

        UnsuscribeToNavigationEvents();

        sceneNavigator.OnButtonPressed_ExitGame += ExitGame;
        sceneNavigator.OnButtonPressed_StartGame += StartGame;
        sceneNavigator.OnButtonPressed_TitleScreen += TitleScreen;
    }
    private void UnsuscribeToNavigationEvents()
    {
        sceneNavigator.OnButtonPressed_ExitGame -= ExitGame;
        sceneNavigator.OnButtonPressed_StartGame -= StartGame;
        sceneNavigator.OnButtonPressed_TitleScreen -= TitleScreen;
    }
    #endregion





    #region Scene Navigation Buttons
    private void StartGame()
    {
        // se lanza unicamente en la titleScreen
        // descarga TitleScreen
        // carga core, ottagono, water, mainlandExterior,
        
        CursorManager.DisableCursor();

        scenesToLoad.Add(SceneManager.LoadSceneAsync(scenesByName[SceneNames.MainlandExterior]));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(scenesByName[SceneNames.Ottagono], LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(scenesByName[SceneNames.Core], LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(scenesByName[SceneNames.Water], LoadSceneMode.Additive));

        foreach(AsyncOperation op in scenesToLoad) op.allowSceneActivation = false;

        StartCoroutine(LoadMultipleScenes());
        SceneManager.UnloadSceneAsync(scenesByName[SceneNames.TitleScreen]);
    }
    private void ExitGame()
    {
        Application.Quit();
    }
    private void TitleScreen()
    {
        CursorManager.EnableCursor();
        //SceneManager.LoadScene(titleScreenScene);

        // fade a negro
        // activar la camara de titleScreen como principal de nuevo
        // resetear al jugador y todos los enemigos y objetos en la posicion inicial
        // cargar la escena de titleScreen
        // salir del fade en negro
    }
    #endregion






    private IEnumerator LoadMultipleScenes()
    {
        foreach(AsyncOperation op in scenesToLoad)
        {
            Debug.Log(op.progress);
            op.allowSceneActivation = true;
            yield return null;
        }
        scenesToLoad.Clear();

        OnSceneLoaded?.Invoke();
    }

}
