using System;
using TMPro;
using UnityEngine;

public class DefeatScreen : MonoBehaviour
{
    public static event Action OnButtonPressed_Retry;
    public static event Action OnButtonPressed_TitleScreen;
    public static event Action OnButtonPressed_ExitGame;

    private GameObject defeatMessage;
    private GameObject buttonsContainer;


    private void Awake()
    {
        defeatMessage = transform.Find("Defeat").gameObject;
        buttonsContainer = transform.Find("ButtonsContainer").gameObject;
    }
    private void OnEnable()
    {
        BlackScreen.OnBlackScreenVisible += ShowButtons;
    }
    private void OnDisable()
    {
        BlackScreen.OnBlackScreenVisible -= ShowButtons;
    }



    private void ShowButtons()
    {
        if (FindAnyObjectByType<Player>().Lives > 0)
        {
            defeatMessage.SetActive(true);
            buttonsContainer.SetActive(true);
            CursorManager.EnableCursor();
        }
    }
    private void HideButtons()
    {
        defeatMessage.SetActive(false);
        buttonsContainer.SetActive(false);
        this.gameObject.SetActive(false);
        CursorManager.DisableCursor();
    }




    public void Retry()
    {
        RespawnManager.Instance.RespawnPlayer();
        OnButtonPressed_Retry?.Invoke();
        HideButtons();
    }
    public void GoToTitleScreen()
    {
        SceneLoader.Instance.GoToTitleScreen();
        OnButtonPressed_TitleScreen?.Invoke();
        HideButtons();

    }
    public void ExitGame()
    {
        SceneLoader.Instance.ExitGame();
        OnButtonPressed_ExitGame?.Invoke();
        HideButtons();

    }
}
