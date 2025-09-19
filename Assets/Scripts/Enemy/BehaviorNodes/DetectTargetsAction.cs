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
        // Asegúrate de que el objeto TargetsDetector exista antes de intentar usarlo.
        if (TargetsDetector.Value != null)
        {
            TargetsDetector.Value.OnTargetsUpdated += ChangeSelectedTarget;
            return Status.Running;
        }

        // Si el objeto es nulo, la acción no puede ejecutarse.
        Debug.LogError("TargetsDetector is not assigned in the Behavior Tree.");
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return Status.Running;
    }

    protected override void OnEnd()
    {
        
        // Se añade una verificación de nulidad para evitar el error.
        if (TargetsDetector.Value != null)
        {
            TargetsDetector.Value.OnTargetsUpdated -= ChangeSelectedTarget;
        }
    }

    private void ChangeSelectedTarget(GameObject newTarget)
    {
        if (SelectedTarget != null)
        {
            SelectedTarget.Value = newTarget;

            if (SelectedTarget.Value == null)
            {
                return;
            }

            if (newTarget != SelectedTarget.Value) SelectedTarget.Value = newTarget;
        }
    }
}