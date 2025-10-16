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
    [SerializeReference] public BlackboardVariable<float> CurrentHealth;
    [SerializeReference] public BlackboardVariable<EnemyStates> CurrentState;


    private Enemy enemy;

    private bool damageRecieved;

    protected override Status OnStart()
    {
        if(CurrentState.Value == EnemyStates.Dead) return Status.Success;

        damageRecieved = false;

        enemy = Self.Value.GetComponent<Enemy>();
        if(enemy != null) enemy.OnDamageRecieved += RecieveDamage;


        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        return damageRecieved ? Status.Success : Status.Running;
    }
    protected override void OnEnd()
    {
        if (enemy != null) enemy.OnDamageRecieved -= RecieveDamage;
    }




    private void RecieveDamage(Unit unit, int currentHealth)
    {
        CurrentHealth.Value = currentHealth;
        damageRecieved = true;
    }
}

