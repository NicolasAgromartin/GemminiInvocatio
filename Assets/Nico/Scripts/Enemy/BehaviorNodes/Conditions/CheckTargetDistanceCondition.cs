using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTargetDistance", story: "Check if the SelectedTartet is at [AttackDistance]", category: "Conditions", id: "3a142619a5f2ceeee5c4d961ffa74dac")]
public partial class CheckTargetDistanceCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> AttackDistance;

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
