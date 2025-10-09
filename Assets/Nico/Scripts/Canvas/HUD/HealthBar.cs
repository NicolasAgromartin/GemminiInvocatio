using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    private TMP_Text healthValue;


    private void Awake()
    {
        healthBar = transform.Find("Image").GetComponent<Image>();
        healthValue = GetComponentInChildren<TMP_Text>();
    }




    public void ChangeHealth(int newHealth)
    {
        if(newHealth < 0) newHealth = 0;
        healthValue.text = $"{newHealth}% HP";
        StartCoroutine(ChangeHealthBar(newHealth));
    }
    private IEnumerator ChangeHealthBar(int newHealth)
    {
        //if (newHealth > 100) yield return null;
        //else
        {
            float target = newHealth / 100f;
            float speed = 0.01f;

            while (!Mathf.Approximately(healthBar.fillAmount, target))
            {
                healthBar.fillAmount = Mathf.MoveTowards(
                    healthBar.fillAmount,
                    target,
                    speed
                );

                yield return null;
            }
        }
    }
}
