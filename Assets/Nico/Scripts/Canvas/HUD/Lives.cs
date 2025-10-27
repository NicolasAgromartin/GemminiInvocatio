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
    }

    private void DeactivateLiveSphere(int liveCount)
    {
        lives[liveCount].transform.Find("LiveSphere").gameObject.SetActive(false);
    }



    
}
