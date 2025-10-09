using System.Collections;
using UnityEngine;
using TMPro;

public class StoryText : MonoBehaviour
{
    public TextMeshProUGUI textUI;   
    [TextArea(3, 10)]                
    public string[] frases;          

    public float delayEntreLetras = 0.05f;
    public float delayEntreFrases = 1.5f;

    void Start()
    {
        StartCoroutine(MostrarTexto());
    }

    IEnumerator MostrarTexto()
    {
        foreach (string frase in frases)
        {
            textUI.text = "";

            foreach (char letra in frase.ToCharArray())
            {
                textUI.text += letra;
                yield return new WaitForSeconds(delayEntreLetras);
            }

            yield return new WaitForSeconds(delayEntreFrases);
        }
    }
}