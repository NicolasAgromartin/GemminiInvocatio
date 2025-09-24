using UnityEngine;

public class EnemiN0 : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani; 
    public Quaternion angulo;
    public float grado;
    public Transform player; 
    public float distanciaVision = 10f;
    

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        Comportamiento_Enemigo();
    }

    void Comportamiento_Enemigo()
    {
        
        float distancia = Vector3.Distance(transform.position, player.position);
        if (distancia <= distanciaVision)
        {
            
            transform.LookAt(player);
            transform.Translate(Vector3.forward * 2f * Time.deltaTime);
            ani.SetBool("Walk", true);
        }
        else
        {
            
            cronometro += Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 3);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    ani.SetBool("Walk", false);
                    break;
                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;
                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 1f * Time.deltaTime);
                    ani.SetBool("Walk", true);
                    break;
            }
        }
    }
}