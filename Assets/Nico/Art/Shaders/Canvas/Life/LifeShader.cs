using UnityEngine;
using UnityEngine.UI;




public class LifeShader : MonoBehaviour
{
    private Material originalMaterial;
    private Material material;

    private float random;




    private void Awake()
    {
        originalMaterial = GetComponent<Image>().material;
        material = Instantiate(originalMaterial);

        GetComponent<Image>().material = material;
    }
    private void Start()
    {
        random = Random.Range(.1f, .5f);

        material.SetFloat("_RandomTime", random);
    }



    public void TurnOff()
    {
        material.SetColor("_Color", Color.black);
    }
    public void TurnOn()
    {

    }
}

