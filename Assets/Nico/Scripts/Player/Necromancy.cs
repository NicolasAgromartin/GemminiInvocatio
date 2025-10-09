using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;






public class Necromancy : MonoBehaviour
{
    #region Events
    public static event Action<List<SummonName>> OnRitualsObtained;
    public static event Action<PlayerMinion> OnNewMinionCreated;
    #endregion


    [Header("Prefabs")]
    [SerializeField] private GameObject playerMinion;
    [SerializeField] private GameObject invocationAModel;
    [SerializeField] private GameObject invocationBModel;
    [SerializeField] private GameObject skeletonPrefab;

    [Header("Fiends SO")]
    [SerializeField] private FiendSO skeletonData;
    [SerializeField] private FiendSO invocationA;
    [SerializeField] private FiendSO invocationB;

    [Header("Ritual Materials")]
    [SerializeField] private RitualMaterial_SO blood;
    [SerializeField] private RitualMaterial_SO skull;
    [SerializeField] private RitualMaterial_SO bone;
    [SerializeField] private RitualMaterial_SO heart;
    [SerializeField] private RitualMaterial_SO skin;
    [SerializeField] private RitualMaterial_SO ashes;


    [Header("Components")]
    [SerializeField] private Inventory inventory;


    private Dictionary<ItemType, RitualMaterial_SO> lootableMaterials;
    private Remains remains;








    private void Awake()
    {
        lootableMaterials = new()
        {
            { ItemType.Heart, heart },
            { ItemType.Blood, blood },
            { ItemType.Bone, bone },
            { ItemType.Skull, skull },
            { ItemType.Skin, skin },
            { ItemType.Ashes, ashes },
        };
    }
    private void OnEnable()
    {
        NecromancyScreen.OnButtonPressed_Resurrect += Resurrect;
        NecromancyScreen.OnButtonPressed_UseSkeleton += UseSkeleton;
        NecromancyScreen.OnButtonPressed_Disect += Disect;

        RitualPanel.OnPanelOpened += ShowRitualOptions;
        RitualPanel.OnButtonPressed_Summon += Summon;
    }
    private void OnDisable()
    {
        NecromancyScreen.OnButtonPressed_Resurrect -= Resurrect;
        NecromancyScreen.OnButtonPressed_UseSkeleton -= UseSkeleton;
        NecromancyScreen.OnButtonPressed_Disect -= Disect;

        RitualPanel.OnPanelOpened -= ShowRitualOptions;
        RitualPanel.OnButtonPressed_Summon -= Summon;
    }



    #region Resurrect
    private void Resurrect()
    {
        GameObject minion = remains.gameObject;

        minion.tag = "PlayerMinion";
        minion.name = "PlayerMinion - " + $"{remains.Data.name}";

        PlayerMinion resurrectedMinion = minion.AddComponent<PlayerMinion>();
        resurrectedMinion.SetMinionData(remains.Data);

        OnNewMinionCreated?.Invoke(resurrectedMinion);
    }
    #endregion


    #region Use Skeleton
    private void UseSkeleton()
    {
        GameObject minion = remains.gameObject;
        RemoveModel(remains.transform); // eliminarlo antes de instanciar el nuevo modelo

        minion.tag = "PlayerMinion";
        minion.name = "PlayerMinion - " + $"{remains.Data.name}";

        Instantiate(skeletonPrefab, minion.transform);

        PlayerMinion skeleton = minion.AddComponent<PlayerMinion>();
        skeleton.SetMinionData(skeletonData);

        OnNewMinionCreated?.Invoke(skeleton);

    }
    private void RemoveModel(Transform remains)
    {
        GameObject model = null;

        foreach (Transform child in remains)
        {
            if (child.gameObject.CompareTag("FiendModel"))
            {
                model = child.gameObject;
                break;
            }
        }

        if (model != null) Destroy(model);
    }
    #endregion



    #region Summons
    private void ShowRitualOptions()
    {
        List<SummonName> possibleSummons = new();

        Dictionary<ItemType, List<Item>> playerItems = inventory.GetAllItems();
        FiendType corpseType = remains.Data.type;
        List<RitualCombination> combinations = new();

        foreach (KeyValuePair<SummonName, (FiendType, List<ItemType>)> ritual in Dictionaries.SummonByMaterials)
        {
            combinations.Add(new RitualCombination(ritual.Value.Item1, ritual.Value.Item2, ritual.Key));
        }


        foreach (RitualCombination combination in combinations)
        {
            if (combination.corpseType != corpseType) continue;

            bool hasAllItems = true;
            foreach (ItemType req in combination.materials)
            {
                if (!playerItems.ContainsKey(req) || playerItems[req].Count == 0)
                {
                    hasAllItems = false;
                    break;
                }
            }

            if (hasAllItems) possibleSummons.Add(combination.summonName);
        }

        OnRitualsObtained?.Invoke(possibleSummons);
    }

    private void Summon(SummonName summon)
    {
        GameObject minion = remains.gameObject;
        minion.tag = "PlayerMinion";
        minion.name = "PlayerMinion - " + $"{remains.Data.name}";

        RemoveModel(remains.transform); // eliminarlo antes de instanciar el nuevo modelo
        PlayerMinion newMinion = minion.AddComponent<PlayerMinion>();

        if (summon == SummonName.SummonA)
        {
            Instantiate(invocationAModel, minion.transform);
            newMinion.SetMinionData(invocationA);
        }
        if (summon == SummonName.SummonB)
        {
            Instantiate(invocationBModel, minion.transform);
            newMinion.SetMinionData(invocationB);
        }

        // elimino los recursos de la lsita del inventario
        List<ItemType> itemsToRemove = Dictionaries.SummonByMaterials[summon].Item2;
        foreach (ItemType item in itemsToRemove) inventory.RemoveItemByType(item);

        OnNewMinionCreated?.Invoke(newMinion);
    }
    #endregion



    #region Disect
    private void Disect()
    {
        FiendType corpseType = remains.Data.type;

        Dictionaries.LootFromCorpse.TryGetValue(corpseType, out List<ItemType> materials);

        foreach (ItemType item in materials)
        {
            inventory.AddItem(new(lootableMaterials[item]));
        }

        Destroy(remains.gameObject);
    }
    #endregion


    #region Setters
    public void SetRemains(Remains remains) => this.remains = remains;
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        //LoadInventory();
    }
    #endregion




    private void LoadInventory()
    {
        for (int i = 0; i < 5; i++)
        {
            inventory.AddItem(new(heart));
        }
        for (int i = 0; i < 5; i++)
        {
            inventory.AddItem(new(blood));
        }
        for (int i = 0; i < 5; i++)
        {
            inventory.AddItem(new(bone));
        }
        for (int i = 0; i < 5; i++)
        {
            inventory.AddItem(new(skull));
        }
        for (int i = 0; i < 5; i++)
        {
            inventory.AddItem(new(skin));
        }
        for (int i = 0; i < 5; i++)
        {
            inventory.AddItem(new(ashes));
        }

    }



}


public struct RitualCombination
{
    public FiendType corpseType;
    public List<ItemType> materials;
    public SummonName summonName;

    public RitualCombination(FiendType corpseType, List<ItemType> materials, SummonName summonName)
    {
        this.corpseType = corpseType;
        this.materials = materials;
        this.summonName = summonName;
    }
}
