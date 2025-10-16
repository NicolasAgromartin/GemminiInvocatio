using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetBlackboardParameters", story: "Get parameters from [Self]", category: "Action", id: "c05076b63508edf9cfa3055071cb7b59")]
public partial class GetBlackboardParametersAction : Action
{
    [SerializeReference] public BlackboardVariable<TargetsDetector> TargetsDetector;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<GameObject> Self;


    protected override Status OnStart()
    {
        TargetsDetector.Value = Self.Value.GetComponentInChildren<TargetsDetector>();
        Animator.Value = Self.Value.GetComponentInChildren<Animator>();

        return Status.Success;
    }
}

