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




    public void ChangeHealth(int newHealth)
    {
        if(newHealth < 0) newHealth = 0;
        StartCoroutine(ChangeHealthBar(newHealth));
    }
    private IEnumerator ChangeHealthBar(int newHealth)
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
