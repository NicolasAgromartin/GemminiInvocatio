using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;




[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Death", story: "[Self] listens to death event", category: "Action", id: "0b5ed8939ebc59f87f8f4a4f543bb6cc")]
public partial class DeathAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;


    protected override Status OnStart()
    {
        GameObject.Destroy(Self.Value.GetComponent<BehaviorGraphAgent>());
        return Status.Success;
    }

}

