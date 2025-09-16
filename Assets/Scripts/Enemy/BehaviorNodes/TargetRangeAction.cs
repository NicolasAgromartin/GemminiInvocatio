using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TargetRange", story: "Check if [SelectedTarget] is [InAttackRange] of [Self]", category: "Action", id: "e266f7316b5922f8651e862ab5f24e92")]
public partial class TargetRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> InAttackRange;
    [SerializeReference] public BlackboardVariable<GameObject> SelectedTarget;

    [SerializeReference] public BlackboardVariable<GameObject> Self;


    protected override Status OnStart()
    { 
        /*
         check if player is far away than the attacking distance of self
         */

        float distance = Vector3.Distance(Self.Value.transform.position, SelectedTarget.Value.transform.position);

        if (distance < 2f)
        {
            InAttackRange.Value = true;
        }

        Debug.Log(distance);

        return Status.Success;
        //return inRange? Status.Success : Status.Failure;
    }

}

