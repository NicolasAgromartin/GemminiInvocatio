using UnityEngine;
using System;



public class SceneNavigator : MonoBehaviour
{
    #region Events
    public event Action OnButtonPressed_TitleScreen;
    public event Action OnButtonPressed_StartGame;
    public event Action OnButtonPressed_ExitGame;    
    #endregion



    public void Play()
    {
        OnButtonPressed_StartGame?.Invoke();
    }
    public void ExitGame()
    {
        OnButtonPressed_ExitGame?.Invoke();
    }
    public void TitleScreen()
    {
        OnButtonPressed_TitleScreen?.Invoke();
    }

}
