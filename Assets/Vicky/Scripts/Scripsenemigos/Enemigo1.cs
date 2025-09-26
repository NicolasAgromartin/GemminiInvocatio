using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemigo1 : Enemigo0
{
    private NavMeshAgent agente;
    public Animator animaciones; 

    private new void Awake()
    {
        base.Awake();

        agente = GetComponent<NavMeshAgent>();
    }


    public new void EstadoIdle()
    {
        base.EstadoIdle();
        if (animaciones != null) animaciones.SetFloat("Velocity", 0);
        if (animaciones != null) animaciones.SetBool("Atacando", false);
        agente.SetDestination(transform.position);
    }

    public new void EstadoSeguir()
    {
        base.EstadoSeguir();
        if (animaciones != null) animaciones.SetFloat("Velocity", 1);
        if (animaciones != null) animaciones.SetBool("Atacando", false);
        if (target != null)
        {
            agente.SetDestination(target.position);
        }
    }

    public new void EstadoAtacar()
    {
        base.EstadoAtacar();
        if (animaciones != null) animaciones.SetFloat("Velocity", 0);
        if (animaciones != null) animaciones.SetBool("Atacando", true);
        if (target != null)
        {
            agente.SetDestination(transform.position);
            transform.LookAt(target, Vector3.up);
        }
    }

    public new void EstadoMuerto()
    {
        base.EstadoMuerto();
        if (animaciones != null) animaciones.SetBool("Vivo", false);
        agente.enabled = false;
    }

   /* public void Matar()
    {
        CambiarEstados(Estados.Matar());  
    }*/
}
