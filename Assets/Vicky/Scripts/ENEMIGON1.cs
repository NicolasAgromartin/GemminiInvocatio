using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class ENEMIGON1 : Enemigo1
{
    private NavMeshAgent agente;

    private new void Awake()
    {
        base.Awake();

        agente = GetComponent<NavMeshAgent>();
    }


    public new void EstadoIdle()
    {
        base.EstadoIdle();

        agente.SetDestination(transform.position);
    }

    public new void EstadoSeguir()
    {
        base.EstadoSeguir();
        if (target != null)
        {
            agente.SetDestination(target.position);
        }
    }

    public new void EstadoAtacar()
    {
        base.EstadoAtacar();
        if (target != null)
        {
            agente.SetDestination(transform.position);
            transform.LookAt(target, Vector3.up);
        }
    }

    public new void EstadoMuerto()
    {
        base.EstadoMuerto();
        agente.enabled = false;
    }

}

