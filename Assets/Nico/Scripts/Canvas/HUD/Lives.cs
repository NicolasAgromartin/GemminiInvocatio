using UnityEngine;
using System.Collections.Generic;





public class Lives : MonoBehaviour
{
    private readonly Dictionary<int, GameObject> lives = new();

    private void Awake()
    {
        int count = 0;

        foreach(Transform live in transform)
        {
            count++;
            lives.Add(count, live.gameObject);
        }
    }


    // newlives = 2 --> elimino 1
    // newLives = 1 --> Elimino 2
    // newLives = 0 --> Elimino 3

    public void ChangeLives(int newLives)
    {
        Debug.Log(newLives);

        switch (newLives)
        {
            case 2: 
                DeactivateLiveSphere(1); 
                break;
            case 1:
                DeactivateLiveSphere(2); 
                break;
            case 0:
                DeactivateLiveSphere(3);
                break;
        }
        // tengo una lista ocn todos los gameobjects
        // cada gameObject representa 1 vida

        // busco entre los gameObjects los activos y elimino uno
        // o defino para cada gameobject un numnero
        // cuando pierdo una vida desactivo ese numero del diccionario
    }

    private void DeactivateLiveSphere(int liveCount)
    {
        lives[liveCount].transform.Find("LiveSphere").gameObject.SetActive(false);
    }



    
}
