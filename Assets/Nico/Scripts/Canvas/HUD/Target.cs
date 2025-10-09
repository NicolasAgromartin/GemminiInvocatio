using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    private TMP_Text focusedTarget;



    private void Awake()
    {
        focusedTarget = transform.Find("FocusedTarget").GetComponent<TMP_Text>();
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
            focusedTarget.text = string.Empty;
        }
        else
        {
            focusedTarget.text = newTarget.name;
        }
    }
}
