using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public static event Action OnBlackScreenVisible;
    public static event Action OnBlackScreenHidden;

    private readonly float fadeSpeed = .8f;
    private Color currecntColor;
    private Image blackScreen;



    private void Awake()
    {
        blackScreen = GetComponent<Image>();
    }
    private void OnEnable()
    {
        AnimationEvents.OnDeathAnimationEnd += ShowBlackScreen;
        Player.OnPlayerRestored += HideBlackScreen;

        // desde el scene loader cuando haya una transicion de escenas 
        // tambien tiene que haber un fade a negro? 
    }
    private void OnDisable()
    {
        AnimationEvents.OnDeathAnimationEnd -= ShowBlackScreen;
        Player.OnPlayerRestored -= HideBlackScreen;

    }


    private void ShowBlackScreen()
    {
        StartCoroutine(FadeIn());
    }
    private void HideBlackScreen()
    {
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
        OnBlackScreenVisible?.Invoke();
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
        OnBlackScreenHidden?.Invoke();
    }

}
