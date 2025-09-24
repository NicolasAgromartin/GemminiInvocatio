using System;
using System.Collections.Generic;

public static class Dictionaries
{
    public static Dictionary<SummonName, (FiendType, List<ItemType>)> SummonByMaterials = new()
    {
        { SummonName.SummonA, (FiendType.Zombie, new List<ItemType> { ItemType.Heart, ItemType.Skull }) },
        { SummonName.SummonB, (FiendType.Skeleton, new List<ItemType> { ItemType.Skull }) },
    };

    public static Dictionary<(FiendType, List<ItemType>), SummonName> MaterialsBySummon = new()
    {
        { (FiendType.Skeleton, new List<ItemType> { ItemType.Heart, ItemType.Skull }), SummonName.SummonA},
        { (FiendType.Skeleton, new List<ItemType> { ItemType.Skull }), SummonName.SummonB },
    };

    public static Dictionary<FiendType, List<ItemType>> LootFromCorpse = new()
    {
        { FiendType.Skeleton, new List<ItemType> { ItemType.Bone, ItemType.Skull } },
        { FiendType.Zombie, new List<ItemType> { ItemType.Blood, ItemType.Skin, ItemType.Heart, ItemType.Bone, ItemType.Skull } },
        { FiendType.Creature, new List<ItemType> { ItemType.Blood, ItemType.Skull } },
        { FiendType.Demon, new List<ItemType> { ItemType.Blood, ItemType.Bone, ItemType.Skull } }
    };



}
