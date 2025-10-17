using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class Lives : MonoBehaviour
{
    private readonly List<LifeShader> lives = new();



    private void Awake()
    {
        lives.AddRange(GetComponentsInChildren<LifeShader>());
    }




    public void ChangeLives(int newLives)
    {
        Debug.Log(newLives);
        lives.First().TurnOff();
    }
}
