using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class LoadingScreen : MonoBehaviour
{
    private Image loadingBar;
    private Image blackScreen;

    private readonly float fadeSpeed = .8f;
    private Color currecntColor;



    private void Awake()
    {
        blackScreen = GetComponent<Image>();
        loadingBar = transform.Find("LoadingBar").GetComponent<Image>();
    }



    public IEnumerator ShowBlackScreen()
    {
        loadingBar.enabled = true;
        blackScreen.enabled = true;
        currecntColor = blackScreen.color;

        while (currecntColor.a < 1f)
        {
            currecntColor.a += Time.unscaledDeltaTime * fadeSpeed;
            blackScreen.color = currecntColor;
            yield return null;
        }

        currecntColor.a = 1f;
        blackScreen.color = currecntColor;
    }

    public void LoadBar(float amount)
    {
        loadingBar.fillAmount = amount;
    }
    public void HideLoadBar()
    {
        loadingBar.enabled = false;
        loadingBar.fillAmount = 0;
    }

    public IEnumerator HideBlackScreen()
    {

        currecntColor = blackScreen.color;
        loadingBar.enabled = false;

        while (currecntColor.a > 0f)
        {
            currecntColor.a -= Time.unscaledDeltaTime * fadeSpeed;
            blackScreen.color = currecntColor;
            yield return null;
        }

        currecntColor.a = 0f;
        blackScreen.color = currecntColor;
        blackScreen.enabled = false;
    }
}
