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
    }
    private void OnDisable()
    {
        InputManager.OnPauseButtonPressed -= ToggleGamePause;
        pauseScreen.OnButtonPressed_ResumeGame -= ToggleGamePause;
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
            //pausedSnapshot.TransitionTo(.5f);
            //Debug.Log("Change th fkin m");

            //mixer.TransitionToSnapshots(
            //    new[] { unpausedSnapshot, pausedSnapshot }, 
            //    new float[] { 1f, 0f }, 
            //    .5f);

        }
        else
        {
            CursorManager.DisableCursor();
            Time.timeScale = 1.0f;
            //unpausedSnapshot.TransitionTo(.5f);
            //Debug.Log("Change itback");

        }
    }
}
