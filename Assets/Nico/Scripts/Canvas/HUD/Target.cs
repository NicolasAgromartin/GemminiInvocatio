using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private TMP_Text target;
    private Image targetIcon;
    private HealthBar health;

    private Unit unit;



    private void Awake()
    {
        target = transform.Find("TargetName").GetComponent<TMP_Text>();
        targetIcon = transform.Find("TargetIcon").GetComponent<Image>();
        health = GetComponentInChildren<HealthBar>();
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
        if(unit != null) unit.OnDamageRecieved -= ChangeTargetHealt;


        if (newTarget == null)
        {
            target.text = string.Empty;
        }
        else
        {
            unit = newTarget.GetComponent<Unit>();

            target.text = unit.name;
            health.SetInitialHealth(unit.Stats.CurrentHealth, unit.Stats.MaxHealth);
            // unit.data.type y seteo el icono correspondiente de su tipo

            unit.OnDamageRecieved += ChangeTargetHealt;
        }
    }



    private void ChangeTargetHealt(Unit target, int newHealth)
    {
        health.ChangeHealth(newHealth, target.Stats.MaxHealth);
    }
}
