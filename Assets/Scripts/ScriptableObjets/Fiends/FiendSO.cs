using UnityEngine;

[CreateAssetMenu(fileName = "FiendSO", menuName = "Scriptable Objects/Fiend")]
public class FiendSO : ScriptableObject
{
    [Header("Model")]
    public GameObject modelPrefab;

    [Header("Basic Data")]
    public string fiendName;
    public FiendType type;
    public Stats stats;

    [Header("Agent Movement")]
    public float baseOffset;
    public float speed;
    public float angularSpeed;
    public float acceleration;
    public float stoppingDistance; // attack distance?
    public int priority;

    [Header("Agent Size")]
    public float radius;
    public float height;

    [Header("Combat")]
    public float timeBetweenAttacks;
    public float attackRange;
}
