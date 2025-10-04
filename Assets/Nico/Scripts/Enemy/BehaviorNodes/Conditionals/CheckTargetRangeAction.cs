using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckTargetRange", story: "Check if the [SelectedTarget] is in [AttackDIstance]", category: "Flow/Conditional", id: "8ccf44c94d6e409b39d270aca50ec647")]
public partial class CheckTargetRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> SelectedTarget;
    [SerializeReference] public BlackboardVariable<float> AttackDIstance;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

