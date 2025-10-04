using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;




[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DetectTargets", story: "Use [TargetsDetector] to get [SelectedTarget]", category: "Action", id: "a0f9e7f830cbf043aa1624af6b84048d")]
public partial class DetectTargetsAction : Action
{
    [SerializeReference] public BlackboardVariable<TargetsDetector> TargetsDetector;
    [SerializeReference] public BlackboardVariable<GameObject> SelectedTarget;




    protected override Status OnStart()
    {
        TargetsDetector.Value.OnTargetsUpdated += ChangeSelectedTarget;
        return Status.Running;
    }
    protected override Status OnUpdate()
    { 
        return Status.Running;
    }
    protected override void OnEnd()
    {
        //Debug.Log("Unsiscribed to the target detector");
        TargetsDetector.Value.OnTargetsUpdated -= ChangeSelectedTarget;
    }





    private void ChangeSelectedTarget(GameObject newTarget)
    {
        SelectedTarget.Value = newTarget;

        if (SelectedTarget.Value == null) return;

        if (newTarget != SelectedTarget.Value) SelectedTarget.Value = newTarget;

        //Debug.Log($"{TargetSelected.Value} <---___--> {SelectedTarget.Value}");
    }
}

