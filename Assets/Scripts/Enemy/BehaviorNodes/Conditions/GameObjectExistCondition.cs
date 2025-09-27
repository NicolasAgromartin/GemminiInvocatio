using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "GameObjectExist", story: "Check if [GameObject] is null", category: "Conditions", id: "83228f89786a73f67cfd233821ecda3b")]
public partial class GameObjectExistCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> gameObject;


    public override bool IsTrue()
    {
        return gameObject.Value == null;
    }

    public override void OnStart()
    {
        IsTrue();
    }
    
}
