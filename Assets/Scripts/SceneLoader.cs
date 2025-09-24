using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SceneLoader : Singleton<SceneLoader>
{
    private event Action<int> OnSceneLoaded;
    private int currentSceneIndex;


    private readonly int titleScreenScene = 0;
    private readonly int mainGameScene = 1;
    private readonly int gameOverScene = 3;


    new private void Awake()
    {
        base.Awake();
        gameObject.transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


        /* Game test  */
        CheckLoadedScene(currentSceneIndex);
    }

    private Player player;
    private void CheckLoadedScene(int loadedScene)
    {
        if (currentSceneIndex == 0)
        {
            ManageTitleScreen();
        }
        if(loadedScene == mainGameScene)
        {
            player = FindAnyObjectByType<Player>();
            player.OnPlayerLost += GoToDefeatScreen;
        }
        if(loadedScene == gameOverScene)
        {
            ManageGameOverScreen();
        }
    }


    #region TitleScreen
    private void ManageTitleScreen()
    {
        Canvas titleScreenCanvas = FindAnyObjectByType<Canvas>();
        Button playButton = titleScreenCanvas.transform.Find("ButtonsContainer/PlayButton").GetComponent<Button>();
        Button exitButton = titleScreenCanvas.transform.Find("ButtonsContainer/ExitButton").GetComponent<Button>();

        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
            OnSceneLoaded?.Invoke(1);
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
        SceneManager.LoadScene(titleScreenScene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void GoToDefeatScreen()
    {
        player.OnPlayerLost -= GoToDefeatScreen;
        SceneManager.LoadScene(3);
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
            OnSceneLoaded?.Invoke(0);
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
