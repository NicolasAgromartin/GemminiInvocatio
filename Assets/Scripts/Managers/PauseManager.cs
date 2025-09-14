using System;
using UnityEngine;



public class PauseManager : Singleton<PauseManager>
{
    public static event Action<bool> OnPauseToggled;

    private bool isGamePaused = false;

    private PauseScreen pauseScreen;
    private Player player;


    #region Life Cykle
    new private void Awake()
    {
        base.Awake();
        pauseScreen = FindAnyObjectByType<PauseScreen>(FindObjectsInactive.Include);
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);
    }
    private void OnEnable()
    {
        InputManager.OnPauseButtonPressed += ToggleGamePause;
        pauseScreen.OnButtonPressed_ResumeGame += ToggleGamePause;
        
        //player.OnLifeLost += ToggleGamePause;
    }
    private void OnDisable()
    {
        InputManager.OnPauseButtonPressed -= ToggleGamePause;
        pauseScreen.OnButtonPressed_ResumeGame -= ToggleGamePause;

        //player.OnLifeLost -= ToggleGamePause;
    }
    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
    #endregion



    private void ToggleGamePause()
    {
        isGamePaused = !isGamePaused;
            
        OnPauseToggled?.Invoke(isGamePaused);

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            CursorManager.EnableCursor();
        }
        else
        {
            CursorManager.DisableCursor();
            Time.timeScale = 1.0f;
        }
    }
}
