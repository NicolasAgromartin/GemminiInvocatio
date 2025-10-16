using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefeatScreen : MonoBehaviour
{
    public static event Action OnButtonPressed_Retry;
    public static event Action OnButtonPressed_TitleScreen;
    public static event Action OnButtonPressed_ExitGame;

    private GameObject defeatMessage;
    private GameObject buttonsContainer;
    private Image blackScreen;
    private Color currecntColor;
    private readonly float fadeSpeed = .8f;





    private void Awake()
    {
        defeatMessage = transform.Find("Defeat").gameObject;
        buttonsContainer = transform.Find("ButtonsContainer").gameObject;
        blackScreen = GetComponent<Image>();
    }
    private void OnEnable()
    {
        ShowBlackScreen();
        Player.OnPlayerRestored += HideBlackScreen;
    }
    private void OnDisable()
    {
        Player.OnPlayerRestored -= HideBlackScreen;
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



    #region UI Buttons
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
    #endregion




    #region Black Screen
    private void ShowBlackScreen()
    {
        StartCoroutine(FadeIn());
    }
    private void HideBlackScreen()
    {
        HideButtons();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        blackScreen.enabled = true;
        currecntColor = blackScreen.color;

        while (currecntColor.a < 1f)
        {
            currecntColor.a += Time.deltaTime * fadeSpeed;
            blackScreen.color = currecntColor;
            yield return null;
        }

        currecntColor.a = 1f;
        blackScreen.color = currecntColor;

        ShowButtons();
    }
    private IEnumerator FadeOut()
    {
        currecntColor = blackScreen.color;

        while (currecntColor.a > 0f)
        {
            currecntColor.a -= Time.deltaTime * fadeSpeed;
            blackScreen.color = currecntColor;
            yield return null;
        }

        currecntColor.a = 0f;
        blackScreen.color = currecntColor;
        blackScreen.enabled = false;
    }
    #endregion
}
