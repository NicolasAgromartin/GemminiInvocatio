using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "[Self] performs an attack using [AttackPerformer]", category: "Action", id: "a72e704d81de12a868260b620363d947")]
public partial class AttackTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<AttackPerformer> AttackPerformer;


    private readonly float damageWindowTime = 1f;
    private bool attackFinished;

    protected override Status OnStart()
    {
        attackFinished = false;
        //AttackPerformer.Value.gameObject.SetActive(true);
        AttackPerformer.Value.StartCoroutine(Attack());
        return Status.Running;
    }

    protected override Status OnUpdate() => attackFinished? Status.Success : Status.Running;


    private IEnumerator Attack()
    {
        yield return AttackPerformer.Value.PerformAttack(damageWindowTime);
        //AttackPerformer.Value.gameObject.SetActive(false);
        attackFinished = true;
    }

}

