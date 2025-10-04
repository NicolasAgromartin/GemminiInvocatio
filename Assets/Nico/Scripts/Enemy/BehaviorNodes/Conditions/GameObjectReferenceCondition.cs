using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "GameObjectReference", story: "[gameObject] reference changed", category: "Variable Conditions", id: "b311aa05a43e6e94558746311d735a1b")]
public partial class GameObjectReferenceCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> gameObject;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
