using System.Collections;
using UnityEngine;
using UnityEngine.UI;




public class HealthBar : MonoBehaviour
{
    private Image healthBar;


    private void Awake()
    {
        healthBar = GetComponent<Image>();
    }



    public void SetInitialHealth(int initialHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)initialHealth / (float)maxHealth;
    }

    public void ChangeHealth(int newHealth, int maxHealth)
    {
        if(newHealth < 0) newHealth = 0;
        StartCoroutine(ChangeHealthBar(newHealth, maxHealth));
    }
    private IEnumerator ChangeHealthBar(int newHealth, int maxHealth)
    {
        float target = (float)newHealth / (float)maxHealth;
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
