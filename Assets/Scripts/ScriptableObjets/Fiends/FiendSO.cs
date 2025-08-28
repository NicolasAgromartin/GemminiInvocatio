using UnityEngine;

[CreateAssetMenu(fileName = "FiendSO", menuName = "Scriptable Objects/Fiend")]
public class FiendSO : ScriptableObject
{
    public string fiendName;
    public FiendType type;

    public GameObject modelPrefab;

    public UnitStats stats;

    public float intervalsBetweenAttacks;


}
