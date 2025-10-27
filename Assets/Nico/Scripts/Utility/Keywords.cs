


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
    Respawn,
}
public enum EnemyEvents
{
    StartPatrol,

    TargetFound,
    TargetDied,
    TargetMoved,
    TargetAtRange,
    TargetLost,

    End,

    Attack,
    TakeHit,
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
    DemonLord,
    Winged,
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
public enum SoundSettings
{
    Master_Volume,
    Music_Volume,
    SFX_Volume,
}
