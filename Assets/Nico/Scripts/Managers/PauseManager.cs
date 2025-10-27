using System;
using UnityEngine;
using UnityEngine.Audio;



public class PauseManager : Singleton<PauseManager>
{
    public static event Action<bool> OnPauseToggled;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Mixer Snapshots")]
    [SerializeField] private AudioMixerSnapshot pausedSnapshot;
    [SerializeField] private AudioMixerSnapshot unpausedSnapshot;

    private bool isGamePaused = false;
    private bool canBePaused = true;

    private PauseScreen pauseScreen;



    #region Life Cykle
    new private void Awake()
    {
        base.Awake();
        pauseScreen = FindAnyObjectByType<PauseScreen>(FindObjectsInactive.Include);
    }
    private void OnEnable()
    {
        InputManager.OnPauseButtonPressed += ToggleGamePause;
        pauseScreen.OnButtonPressed_ResumeGame += ToggleGamePause;

        PlayerStateMachine.OnStateChange += HandleStateChange;
        Player.OnPlayerRestored += EnablePause;
    }
    private void OnDisable()
    {
        InputManager.OnPauseButtonPressed -= ToggleGamePause;
        pauseScreen.OnButtonPressed_ResumeGame -= ToggleGamePause;

        PlayerStateMachine.OnStateChange -= HandleStateChange;
        Player.OnPlayerRestored -= EnablePause;
    }
    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
    #endregion

    private void EnablePause() => canBePaused = true;
    private void DisablePause() => canBePaused = false;
    private void HandleStateChange(BaseState state)
    {
        if (state is PlayerDeadState) DisablePause();
    }



    private void ToggleGamePause()
    {
        //if (PlayerStateMachine.CurrentState is PlayerDeadState) return;
        // evitar que el jugador pueda pausar la partida cuando todabia no se 
        if (!canBePaused) return;

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
