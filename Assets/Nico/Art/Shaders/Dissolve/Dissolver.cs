using UnityEngine;
using System.Collections;


public class Dissolver : MonoBehaviour
{
    private Renderer targetRenderer;
    private Material[] targetMaterials;


    //speed of the dissolve
    [SerializeField] private float dissolveRate = 0.0125f;
    [SerializeField] private float refreshRate = 0.025f;


    private void Awake()
    {
        targetRenderer = this.GetComponent<Renderer>();
        targetMaterials = targetRenderer.materials;
    }




    // 1 desaparece 
    // 0 aparece
    public void Appear()
    {
        StartCoroutine(Dissolve(0f));
    }
    public IEnumerator Dissapear()
    {
        yield return StartCoroutine(Dissolve(1f));
    }




    public IEnumerator Dissolve(float targetValue)
    {
        float current = targetMaterials[0].GetFloat("_VisibleAmount");

        // Decidir dirección: 1 si va aumentando, -1 si va disminuyendo
        float direction = Mathf.Sign(targetValue - current);

        while ((direction > 0 && current < targetValue) || (direction < 0 && current > targetValue))
        {
            current += dissolveRate * direction;
            current = Mathf.Clamp01(current); // Asegura que quede entre 0 y 1

            for (int i = 0; i < targetMaterials.Length; i++)
            {
                targetMaterials[i].SetFloat("_VisibleAmount", current);
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
