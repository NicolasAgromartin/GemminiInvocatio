using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;






public class SceneLoader : Singleton<SceneLoader>
{
    public static event Action OnSceneLoaded;
    public static event Action OnSceneLoading;
    public static event Action OnSceneStartLoading;

    private LoadingScreen loadingScreen;

    public static bool AreScenesLoading { get; private set; }


    public enum SceneNames
    {
        Test,

        TitleScreen,
        GameOverScreen,

        Core,

        Ottagono,
        Water,
        MainlandExterior,

        Hall,
        Hallway,

        FinalDungeon,
    }
    private readonly Dictionary<SceneNames, string> scenesByName = new()
    {
        { SceneNames.Test, "Nico_Test" },

        { SceneNames.TitleScreen, "Nico_TitleScreen" },
        { SceneNames.GameOverScreen ,"Nico_GameOverScreen" },

        { SceneNames.Ottagono, "Nico_Ottagono" },
        { SceneNames.Water, "Nico_Water" },
        { SceneNames.Core, "Nico_Core"  },
        { SceneNames.MainlandExterior, "Nico_MainlandExterior"  },

        { SceneNames.Hall, "Nico_Hall" },
        { SceneNames.Hallway, "Nico_Hallway" },

        { SceneNames.FinalDungeon, "Nico_FinalDungeon"},
    };




    private SceneNavigator sceneNavigator;
    private readonly List<ScenePortal> portals = new();
    private readonly List<AsyncOperation> scenesToLoad = new();
    private readonly List<SceneNames> scenesNeeded = new();






    #region Life Cycle
    new private void Awake()
    {
        base.Awake();
        gameObject.transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        loadingScreen = GetComponentInChildren<LoadingScreen>();

        SuscribeToNavigationEvents();

        OnSceneLoaded += SuscribeToPortals;
        OnSceneLoaded += SuscribeToNavigationEvents;
    }
    private void OnDestroy()
    {
        OnSceneLoaded -= SuscribeToPortals;
        OnSceneLoaded -= SuscribeToNavigationEvents;

        UnsuscribeToNavigationEvents();
    }
    #endregion



    private void SuscribeToPortals()
    {
        if (portals.Count != 0) UnsuscribeToPortals();

        portals.AddRange(FindObjectsByType<ScenePortal>(FindObjectsInactive.Include, FindObjectsSortMode.None));

        foreach(ScenePortal portal in portals)
        {
            portal.OnPortalInteracted += EnterPortal;
        }
    }
    private void UnsuscribeToPortals()
    {
        foreach (ScenePortal portal in portals)
        {
            portal.OnPortalInteracted -= EnterPortal;
        }
    }



    #region Navigation Events
    private void SuscribeToNavigationEvents()
    {
        if(sceneNavigator != null) UnsuscribeToNavigationEvents();

        sceneNavigator = FindAnyObjectByType<SceneNavigator>(FindObjectsInactive.Include);

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





    #region Scene Navigation
    private void StartGame()
    {
        CursorManager.DisableCursor();

        scenesNeeded.Add(SceneNames.Core);
        scenesNeeded.Add(SceneNames.Water);
        scenesNeeded.Add(SceneNames.Ottagono);
        scenesNeeded.Add(SceneNames.MainlandExterior);

        StartCoroutine(TrasitionBetweenScenes(scenesNeeded));
        
    }
    private void ExitGame()
    {
        Application.Quit();
    }
    private void TitleScreen()
    {
        Debug.Log("Go to TitleScreen");

        CursorManager.EnableCursor();

        scenesNeeded.Add(SceneNames.TitleScreen);

        StartCoroutine(TrasitionBetweenScenes(scenesNeeded));
    }
    private void EnterPortal(ScenePortal scenePortal)
    {
        scenesNeeded.AddRange(scenePortal.ScenesToGo);

        StartCoroutine(TrasitionBetweenScenes(scenesNeeded));
    }
    #endregion



    private readonly List<SceneNames> loadedScenes = new();

    private IEnumerator TrasitionBetweenScenes(List<SceneNames> scenesNeeded)
    {
        OnSceneStartLoading?.Invoke();
        AreScenesLoading = true;
        yield return StartCoroutine(loadingScreen.ShowBlackScreen());



        OnSceneLoading?.Invoke();
        yield return UnloadExtraScenes();


        foreach(SceneNames scene in scenesNeeded)
        {
            scenesToLoad.Add(SceneManager.LoadSceneAsync(scenesByName[scene], DefineMode(scene)));
            loadedScenes.Add(scene);
        }

        yield return StartCoroutine(ProgressLoadingBar());
        yield return StartCoroutine(loadingScreen.HideBlackScreen());

        scenesToLoad.Clear();
        scenesNeeded.Clear();
        OnSceneLoaded?.Invoke();
        AreScenesLoading = false;
        Time.timeScale = 1.0f;
    }



    private IEnumerator UnloadExtraScenes()
    {
        List<SceneNames> toUnload = new(loadedScenes);

        foreach (SceneNames scene in toUnload)
        {
            if (scene == SceneNames.Core || scene == SceneNames.TitleScreen || scene == SceneNames.GameOverScreen)
                continue;

            yield return SceneManager.UnloadSceneAsync(scenesByName[scene]);
            loadedScenes.Remove(scene);
        }
    }
    private IEnumerator ProgressLoadingBar()
    {
        float loadProgress = 0f;

        for(int i = 0; i< scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                loadProgress += scenesToLoad[i].progress;
                loadingScreen.LoadBar(loadProgress / scenesToLoad.Count);
                yield return null;
            }
        }

        loadingScreen.HideLoadBar();
    }
    private LoadSceneMode DefineMode(SceneNames scene)
    {
        if(scene == SceneNames.TitleScreen || scene == SceneNames.GameOverScreen)
        {
            CursorManager.EnableCursor();
        }
        if(scene == SceneNames.Core || scene == SceneNames.TitleScreen || scene == SceneNames.GameOverScreen)
        {
            return LoadSceneMode.Single;
        }
        else
        {
            return LoadSceneMode.Additive;
        }
    }
}
