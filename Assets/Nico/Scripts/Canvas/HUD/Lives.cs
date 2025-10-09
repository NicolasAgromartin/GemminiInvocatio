using TMPro;
using UnityEngine;

public class Lives : MonoBehaviour
{
    private TMP_Text livesCounter;


    private void Awake()
    {
        livesCounter = GetComponentInChildren<TMP_Text>();
    }


    public void ChangeLives(int newLives)
    {
        livesCounter.text = newLives.ToString();
    }
}
