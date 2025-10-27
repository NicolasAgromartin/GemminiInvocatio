using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public static class UIEffects
{

    public static IEnumerator FadeIn(Graphic uiElement, float fadeSpeed)
    {
        Color currentColor = uiElement.color;
        uiElement.enabled = true;

        while (currentColor.a < 1f)
        {
            currentColor.a += Time.deltaTime * fadeSpeed;
            uiElement.color = currentColor;
            yield return null;
        }

        currentColor.a = 1f;
        uiElement.color = currentColor;
    }

    public static IEnumerator FadeOut(Graphic uiElement, float fadeSpeed)
    {
        Color currentColor = uiElement.color;

        while (currentColor.a > 0f)
        {
            currentColor.a -= Time.deltaTime * fadeSpeed;
            uiElement.color = currentColor;
            yield return null;
        }

        currentColor.a = 0f;
        uiElement.color = currentColor;
        uiElement.enabled = false;
    }
}
