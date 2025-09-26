using System;
using System.Collections.Generic;
using UnityEngine;






public class Necromancy : MonoBehaviour
{
    #region Events
    public event Action<GameObject> OnUnitResurrected;
    //public event Action<GameObject> OnUnitDisected;
    public event Action<GameObject> OnUnitSummoned;
    public event Action<GameObject> OnUnitDefleshed;
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
    private void Start()
    {
        //LoadInventory();
    }









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
    private void RemoveModel(Transform remains)
    {
        GameObject model = null;

        foreach(Transform child in remains)
        {
            if (child.gameObject.CompareTag("FiendModel"))
            {
                model = child.gameObject;
                break;
            }
        }

        if(model != null) Destroy(model);
    }










    public void SetInventory(Inventory inventory) => this.inventory = inventory;








    public void Resurrect(Remains remains)
    {
        GameObject minion = remains.gameObject;
        
        minion.tag = "PlayerMinion";
        minion.name = "PlayerMinion - " + $"{remains.Data.name}";

        PlayerMinion resurrectedMinion = minion.AddComponent<PlayerMinion>();
        resurrectedMinion.SetMinionData(remains.Data);

        OnUnitResurrected?.Invoke(remains.gameObject);
    }
    public void UseSkeleton(Remains remains)
    {
        GameObject minion = remains.gameObject;
        RemoveModel(remains.transform); // eliminarlo antes de instanciar el nuevo modelo

        minion.tag = "PlayerMinion";
        minion.name = "PlayerMinion - " + $"{remains.Data.name}";

        Instantiate(skeletonPrefab, minion.transform);

        PlayerMinion skeleton = minion.AddComponent<PlayerMinion>();
        skeleton.SetMinionData(skeletonData);

        OnUnitDefleshed?.Invoke(minion);
    }




    #region Summons
    public List<SummonName> GetPossibleSummons(Remains remains)
    {
        List<SummonName> possibleSummons = new();

        Dictionary<ItemType, List<Item>> playerItems = inventory.GetAllItems();
        FiendType corpseType = remains.Data.type;
        List<RitualCombination> combinations = new();

        foreach(KeyValuePair<SummonName, (FiendType, List<ItemType>)> ritual in Dictionaries.SummonByMaterials)
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

        return possibleSummons;
    }
    public void Summon(SummonName summon, Remains remains)
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
        foreach(ItemType item in itemsToRemove) inventory.RemoveItemByType(item);
                
        OnUnitSummoned?.Invoke(minion);
    }
    #endregion



    #region Disect
    public void Disect(GameObject corpse)
    {
        FiendType corpseType = corpse.GetComponent<Remains>().Data.type;

        Dictionaries.LootFromCorpse.TryGetValue(corpseType, out List<ItemType> materials);
        AddMaterialsToInventory(materials);
        Destroy(corpse);
    }
    private void AddMaterialsToInventory(List<ItemType> materials)
    {
        foreach (ItemType item in materials)
        {
            inventory.AddItem(new(lootableMaterials[item]));
        }
    }
    #endregion


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