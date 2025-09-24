/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoBasico : MonoBehaviour
{
    public float velocidadCaminar = 1f;
    public float velocidadCorrer = 2f;
    public float tiempoRutina = 4f;
    public float rangoVision = 5f;
    public float rangoAtaque = 1f;

    private Animator anim;
    private Transform jugador;
    private float cronometro;
    private int rutina;
    private float grado;
    private Quaternion angulo;
    private bool atacando;

    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject objJugador = GameObject.Find("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
        else
        {
            Debug.LogError("No se encontró el objeto 'Player' en la escena.");
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia > rangoVision)
        {
            cronometro += Time.deltaTime;
            if (cronometro >= tiempoRutina)
            {
                rutina = Random.Range(0, 3); 
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    anim.SetBool("caminar", false);
                    break;

                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * velocidadCaminar * Time.deltaTime);
                    anim.SetBool("caminar", true);
                    break;
            }
        }
        else if (distancia > rangoAtaque && distancia <= rangoVision)
        {
            Vector3 direccion = (jugador.position - transform.position).normalized;
            Quaternion rotacion = Quaternion.LookRotation(new Vector3(direccion.x, 0, direccion.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacion, 2f);
            transform.Translate(Vector3.forward * velocidadCorrer * Time.deltaTime);
            anim.SetBool("caminar", true);
        }
        else if (distancia <= rangoAtaque)
        {
            anim.SetBool("caminar", false);
            anim.SetTrigger("atacar");
            
        }
    }
}
*/