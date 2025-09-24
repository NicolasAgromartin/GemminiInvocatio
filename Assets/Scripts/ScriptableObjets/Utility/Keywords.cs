using Unity.Behavior;


//[BlackboardEnum]
//public enum EnemyStates
//{
//    Idle,
//    Patrol,
//    Chase,
//    Attack,
//    Hurt,
//    Dead,
//}
public enum TransitionEvent
{
    Move,
    Interact,
    Tactics,
    TargetSelected,
    TargetLost,
    Attack,
    RecieveDamage,
    End,
    Die,
}
public enum FiendType
{
    Skeleton,
    Creature,
    Zombie,
    Demon,
}
public enum SummonName
{
    SummonA,
    SummonB, 
    SummonC,
}
public enum ItemType
{
    Potion,

    Blood,
    Heart,
    Skull,
    Bone,
    Ashes,
    Skin,

    Letter,
    KeyItem,
    Rune,
    Artifact,

    RitualPage,
    FiendPage,
    SummonPage,
}
public enum MaterialType
{
    Blood,
    Heart,
    Skull,
    Bone,
    Ashes,
    Skin,
}
public enum PageType
{
    RitualPage,
    FiendPage,
    SummonPage,
}
