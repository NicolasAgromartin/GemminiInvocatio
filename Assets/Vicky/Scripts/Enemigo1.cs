using UnityEngine;
using System.Collections;

public class Enemigo1 : MonoBehaviour
{
    public enum Estados
    {
        Idle = 0,
        Seguir = 1,
        Atacar = 2,
        Muerto = 3
    }

    [Header("Estados")]
    public Estados estado;

    [Header("Distancias")]
    public float distanciaSeguir = 5f;
    public float distanciaAtacar = 2f;
    public float distanciaEscape = 8f;

    [Header("Target")]
    public bool autoseleccionTarget = true;
    public Transform target;
    public float distancia;

    [Header("Estado de vida")]
    public bool vivo = true;

    protected virtual void Awake()
    {
        if (autoseleccionTarget)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
                target = jugador.transform;
        }

        StartCoroutine(CalcularDistancia());
    }

    private void LateUpdate()
    {
        CheckStatus();
    }

    private void CheckStatus()
    {
        switch (estado)
        {
            case Estados.Idle:
                EstadoIdle();
                break;
            case Estados.Seguir:
                EstadoSeguir();
                break;
            case Estados.Atacar:
                EstadoAtacar();
                break;
            case Estados.Muerto:
                EstadoMuerto();
                break;
        }
    }

    public virtual void CambiarStatus(Estados nuevoEstado)
    {
        if (nuevoEstado == Estados.Muerto)
            vivo = false;

        estado = nuevoEstado;
    }

    protected virtual void EstadoIdle()
    {
        if (distancia < distanciaSeguir)
            CambiarStatus(Estados.Seguir);
    }

    protected virtual void EstadoSeguir()
    {
        if (distancia < distanciaAtacar)
            CambiarStatus(Estados.Atacar);
        else if (distancia > distanciaEscape)
            CambiarStatus(Estados.Idle);
    }

    protected virtual void EstadoAtacar()
    {
        if (distancia > distanciaAtacar + 0.4f)
            CambiarStatus(Estados.Seguir);
    }

    protected virtual void EstadoMuerto()
    {
        
    }

    private IEnumerator CalcularDistancia()
    {
        while (vivo)
        {
            if (target != null)
                distancia = Vector3.Distance(transform.position, target.position);

            yield return new WaitForSeconds(0.3f);
        }
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtacar);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaSeguir);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaEscape);
    }
}