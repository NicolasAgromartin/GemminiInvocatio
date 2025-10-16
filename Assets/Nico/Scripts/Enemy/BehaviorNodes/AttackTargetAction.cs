using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "[Self] performs an attack", category: "Action", id: "a72e704d81de12a868260b620363d947")]
public partial class AttackTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Animator> Animator;


    //private readonly float damageWindowTime = 1f;
    private bool attackFinished;

    protected override Status OnStart()
    {
        Animator.Value.SetFloat("Movement", 0f);
        
        attackFinished = false;
        return Status.Success;
    }

    protected override Status OnUpdate() => attackFinished? Status.Success : Status.Running;


    private IEnumerator Attack()
    {
        Animator.Value.SetTrigger("Attack");
        yield return null;
        attackFinished = true;
    }

}

