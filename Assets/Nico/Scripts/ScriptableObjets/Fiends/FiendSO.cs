using UnityEngine;

[CreateAssetMenu(fileName = "FiendSO", menuName = "Scriptable Objects/Fiend")]
public class FiendSO : ScriptableObject
{
    [Header("Model")]
    public GameObject modelPrefab;

    [Header("Basic Data")]
    public string fiendName;
    public FiendType type;

    [Header("Unit Stats")]
    public Stats stats;

    public float timeBetweenAttacks;
    public float attackRange;
    public float patrolSpeed;
    public float chaseSpeed;

    public Sprite icon;
}
