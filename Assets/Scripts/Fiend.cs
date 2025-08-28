using UnityEngine;



public class Fiend : Unit
{
    [SerializeField] protected FiendSO data;


    protected virtual void Awake()
    {
        Stats = new(data.stats);

        bool hasModel = false;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("FiendModel"))
            {
                hasModel = true;
                break;
            }
        }
        if (!hasModel) Instantiate(data.modelPrefab, transform);
    }
}
