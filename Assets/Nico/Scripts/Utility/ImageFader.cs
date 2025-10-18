using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ImageFader: MonoBehaviour
{
    protected Image image;

    private readonly float fadeSpeed = .8f;
    private Color currentColor;



    protected virtual IEnumerator FadeIn()
    {
        image.enabled = true;
        currentColor = image.color;

        while (currentColor.a < 1f)
        {
            currentColor.a += Time.deltaTime * fadeSpeed;
            image.color = currentColor;
            yield return null;
        }

        currentColor.a = 1f;
        image.color = currentColor;
    }


    protected virtual IEnumerator FadeOut()
    {
        currentColor = image.color;

        while (currentColor.a > 0f)
        {
            currentColor.a -= Time.deltaTime * fadeSpeed;
            image.color = currentColor;
            yield return null;
        }

        currentColor.a = 0f;
        image.color = currentColor;
        image.enabled = false;
    }


}
