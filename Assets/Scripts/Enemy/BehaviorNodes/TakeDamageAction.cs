using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TakeDamage", story: "[Self] listens to damage recieved", category: "Action", id: "d6378975c76a9f2be9b5a0303d2c84e2")]
public partial class TakeDamageAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;


    private bool damageRecieved;
    protected override Status OnStart()
    {
        damageRecieved = false;

        Self.Value.GetComponent<Enemy>().OnDamageRecievd += WaitForDamage;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return damageRecieved ? Status.Success : Status.Running ;
    }

    protected override void OnEnd()
    {
        Self.Value.GetComponent<Enemy>().OnDamageRecievd -= WaitForDamage;
    }

    private void WaitForDamage() => damageRecieved = true;
}

