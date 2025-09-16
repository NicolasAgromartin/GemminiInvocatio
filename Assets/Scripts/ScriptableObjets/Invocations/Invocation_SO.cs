using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Invocations", menuName = "Scriptable Objects/Invocation")]
public class Invocation_SO : FiendSO
{
    public SummonName summon;

    [Header("Corpse Required")]
    public FiendType corpse;

    [Header("Materials Required")]
    public List<ItemType> materials;


}