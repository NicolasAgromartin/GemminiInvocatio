using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private TMP_Text targetName;
    private Image targetIcon;
    private HealthBar health;
    private Image box;

    private Fiend target;



    private void Awake()
    {
        targetName = transform.Find("TargetName").GetComponent<TMP_Text>();
        targetIcon = transform.Find("TargetIcon").GetComponent<Image>();
        box = GetComponent<Image>();
        health = GetComponentInChildren<HealthBar>(true);
    }
    private void OnEnable()
    {
        EnemyDetector.OnTargetChanged += ChangeFocusedTarget;
    }
    private void OnDisable()
    {
        EnemyDetector.OnTargetChanged -= ChangeFocusedTarget;
    }




    private void ChangeFocusedTarget(GameObject newTarget)
    {
        if(target != null) target.OnDamageRecieved -= ChangeTargetHealt; // si habia un target seleccionado de antes me dessuscribo de el


        if (newTarget == null)
        {
            targetName.text = string.Empty;

            health.gameObject.SetActive(false);
            targetName.gameObject.SetActive(false);
            targetIcon.gameObject.SetActive(false);
            box.enabled = false;
        }
        else
        {
            health.gameObject.SetActive(true);
            targetName.gameObject.SetActive(true);
            targetIcon.gameObject.SetActive(true);
            box.enabled = true;

            target = newTarget.GetComponent<Fiend>();

            targetName.text = target.GetFiendName();
            targetIcon.sprite = target.GetFiendIcon();

            health.SetInitialHealth(target.Stats.CurrentHealth, target.Stats.MaxHealth);

            target.OnDamageRecieved += ChangeTargetHealt;
        }
    }



    private void ChangeTargetHealt(Unit target, int newHealth)
    {
        health.ChangeHealth(newHealth, target.Stats.MaxHealth);
    }
}
