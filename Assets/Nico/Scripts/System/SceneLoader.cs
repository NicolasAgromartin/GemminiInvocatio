using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : Singleton<SceneLoader>
{
    public event Action<int> OnSceneLoaded;
    private int currentSceneIndex;



    //private readonly int storySlideScene = 0;
    private readonly int titleScreenScene = 1;
    private readonly int mainGameScene = 2;
    private readonly int gameOverScene = 3;

    private Player player;
    private Enemy finalBoss;







    new private void Awake()
    {
        base.Awake();
        gameObject.transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        /* Game test  */
        CheckLoadedScene(currentSceneIndex);

    }
    private void OnEnable()
    {
        Player.OnPlayerLost += GoToGameOverScreen;
    }
    private void OnDisable()
    {
        Player.OnPlayerLost -= GoToGameOverScreen;
    }








    private void CheckLoadedScene(int loadedScene)
    {
        if (currentSceneIndex == titleScreenScene)
        {
            ManageTitleScreen();
        }
        if (loadedScene == mainGameScene)
        {
            HandleGameScene();
        }
        if(loadedScene == gameOverScene)
        {
            ManageGameOverScreen();
        }
    }







    #region GameScene
    private void HandleGameScene()
    {
        player = FindAnyObjectByType<Player>();
        //Player.OnPlayerLost += GoToDefeatScreen;

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            if (enemy.isFinalBoos)
            {
                finalBoss = enemy;
                SuscribeToFinalBoss();
                break;
            }
        }
    }
    #endregion








    #region TitleScreen
    private void ManageTitleScreen()
    {
        Canvas titleScreenCanvas = FindAnyObjectByType<Canvas>();
        Button playButton = titleScreenCanvas.transform.Find("ButtonsContainer/PlayButton").GetComponent<Button>();
        Button exitButton = titleScreenCanvas.transform.Find("ButtonsContainer/ExitButton").GetComponent<Button>();

        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(mainGameScene);
            OnSceneLoaded?.Invoke(mainGameScene);
            playButton.onClick.RemoveAllListeners();
        });

        exitButton.onClick.AddListener(() =>
        {
            ExitGame();
            exitButton.onClick.RemoveAllListeners();
        });
    }
    #endregion








    #region GameScene
    public void GoToTitleScreen()
    {
        CursorManager.EnableCursor();
        SceneManager.LoadScene(titleScreenScene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void GoToDefeatScreen()
    {
        SceneManager.LoadScene(gameOverScene);
    }
    private void SuscribeToFinalBoss()
    {
        finalBoss.OnFinalBossDefeated += GoToGameOverScreen;
    }
    private void UnsuscribeToFinalBoss()
    {
        finalBoss.OnFinalBossDefeated -= GoToGameOverScreen;
    }
    private void GoToGameOverScreen()
    {
        //UnsuscribeToFinalBoss();
        SceneManager.LoadScene(gameOverScene);
    }
    #endregion










    #region GameOverScreen
    private void ManageGameOverScreen()
    {
        Canvas gameOverScreen = FindAnyObjectByType<Canvas>();
        Button titleScreenButton = gameOverScreen.transform.Find("ButtonsContainer/TitleScreen_Button").GetComponent<Button>();
        Button exitButton = gameOverScreen.transform.Find("ButtonsContainer/Exit_Button").GetComponent<Button>();

        CursorManager.EnableCursor();
        
        titleScreenButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(titleScreenScene);
            OnSceneLoaded?.Invoke(titleScreenScene);
            titleScreenButton.onClick.RemoveAllListeners();
        });

        exitButton.onClick.AddListener(() =>
        {
            ExitGame();
            exitButton.onClick.RemoveAllListeners();
        });
    }
    #endregion






}
