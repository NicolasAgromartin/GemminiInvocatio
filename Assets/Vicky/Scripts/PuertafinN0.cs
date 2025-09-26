using UnityEngine;

public class PuertafinN0 : MonoBehaviour
{
  
    public Animator animator;
    public Collider colliderFisicoPuerta; 
    public GameObject triggerEntrada;     
    public GameObject triggerSalida;      

    private bool puertaAbierta = false;
    private bool puertaBloqueada = false;

    void Start()
    {
        colliderFisicoPuerta.enabled = false; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        
        if (other.gameObject == triggerEntrada && !puertaAbierta)
        {
            AbrirPuerta();
        }

        
        if (other.gameObject == triggerSalida && puertaAbierta && !puertaBloqueada)
        {
            BloquearPuerta();
        }
    }

    void AbrirPuerta()
    {
        puertaAbierta = true;
        animator.SetBool("Abrir", true);
        Debug.Log("Puerta abierta.");
    }

    void BloquearPuerta()
    {
        puertaBloqueada = true;
        animator.SetBool("Abrir", false); 
        colliderFisicoPuerta.enabled = true; 
        triggerEntrada.SetActive(false);     
        Debug.Log("Puerta bloqueada. No se puede volver.");
    }
}

