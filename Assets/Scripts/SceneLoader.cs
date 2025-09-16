using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SceneLoader : Singleton<SceneLoader>
{
    private int currentSceneIndex;

    new private void Awake()
    {
        base.Awake();
        gameObject.transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 0) ManageTitleScreen();
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
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion


}
