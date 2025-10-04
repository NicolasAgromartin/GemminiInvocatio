using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TargetInRange", story: "Is distance between [Target] and [Self] below [Distance]", category: "Conditions", id: "06723826c27fb8e273bd10dc6fc8828e")]
public partial class TargetInRangeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Distance;


    public override bool IsTrue()
    {
        //Debug.Log(Distance.Value);
        //Debug.Log(Vector3.Distance(Target.Value.transform.position, Self.Value.transform.position));

        return Vector3.Distance(Target.Value.transform.position, Self.Value.transform.position) <= Distance.Value;
    }

    public override void OnStart()
    {
        IsTrue();
    }
}
