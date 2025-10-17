using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SearchTarget", story: "[Self] search a [Target]", category: "Action", id: "ff1598f2ababebccd482a765cfc2fef0")]
public partial class SearchTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<TargetsDetector> TargetsDetector;
    


    
    
    protected override Status OnStart()
    {
        TargetsDetector.Value.OnTargetsUpdated += UpdateTarget;
        UpdateTarget(TargetsDetector.Value.SelectedTarget);
        return Status.Running;
    }


    protected override Status OnUpdate()
    {
        return Status.Running;
    }

    private void UpdateTarget(GameObject target)
    {
        //Debug.Log($"New target {target != null}");
        if(target == null)
        {
            Target.Value = null;
        }
        Target.Value = target;
    }
}

