using UnityEngine;
using System.Collections;


public class Dissolver : MonoBehaviour
{
<<<<<<< Updated upstream
    private Renderer targetRenderer;
=======
    //the target object renderer
    private Renderer targetRenderer;


    //all the monkeys matirials
>>>>>>> Stashed changes
    private Material[] targetMaterials;


    //speed of the dissolve
    [SerializeField] private float dissolveRate = 0.0125f;
    [SerializeField] private float refreshRate = 0.025f;


    private void Awake()
    {
        targetRenderer = this.GetComponent<Renderer>();
        targetMaterials = targetRenderer.materials;
    }

<<<<<<< Updated upstream



    // 1 desaparece 
    // 0 aparece
    public void Appear()
    {
        StartCoroutine(Dissolve(0f));
    }
    public void Dissapear()
    {
        StartCoroutine(Dissolve(1f));
    }




    public IEnumerator Dissolve(float targetValue)
    {
        float current = targetMaterials[0].GetFloat("_VisibleAmount");
=======
    // 1 desaparece 
    // 0 aparece
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            StartCoroutine(Dissolve(1f));
        }
    }


    private IEnumerator Dissolve(float targetValue)
    {
        float current = targetMaterials[0].GetFloat("_visble_amount");
>>>>>>> Stashed changes

        // Decidir dirección: 1 si va aumentando, -1 si va disminuyendo
        float direction = Mathf.Sign(targetValue - current);

        while ((direction > 0 && current < targetValue) || (direction < 0 && current > targetValue))
        {
            current += dissolveRate * direction;
            current = Mathf.Clamp01(current); // Asegura que quede entre 0 y 1

            for (int i = 0; i < targetMaterials.Length; i++)
            {
<<<<<<< Updated upstream
                targetMaterials[i].SetFloat("_VisibleAmount", current);
=======
                targetMaterials[i].SetFloat("_visble_amount", current);
>>>>>>> Stashed changes
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
