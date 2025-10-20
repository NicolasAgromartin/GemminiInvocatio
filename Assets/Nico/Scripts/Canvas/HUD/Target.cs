using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private TMP_Text target;
    private Image targetIcon;
    private Image targetHealthBar;




    private void Awake()
    {
        target = transform.Find("TargetName").GetComponent<TMP_Text>();
        targetIcon = transform.Find("TargetIcon").GetComponent<Image>();
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
        if (newTarget == null)
        {
            target.text = string.Empty;
        }
        else
        {
            target.text = newTarget.name;
        }
    }
}
