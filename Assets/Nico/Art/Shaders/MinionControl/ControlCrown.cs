using System.Collections;
using UnityEngine;

public class ControlCrown : MonoBehaviour
{
    private Color color;
    private Material material;

    private float currentAlpha;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;

        color = material.color;
        color.a = 0f;
        material.color = color; 
    }

    private void Start()
    {
        //StartCoroutine(ShowCrown());
    }

    //private IEnumerator ShowCrown()
    //{
    //    while (currentAlpha < 1f)
    //    {
    //        currentAlpha += Time.deltaTime * 0.1f;
    //        color.a = currentAlpha;
    //        material.color = color;
    //        yield return null;
    //    }

    //    color.a = 1f;
    //    material.color = color;
    //}
}