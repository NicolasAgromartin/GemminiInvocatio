using System;
using System.Collections.Generic;
using System.Linq;
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
        };
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
        GameObject minion = Instantiate(playerMinion);

        Instantiate(skeletonPrefab, minion.transform);
        minion.transform.position = remains.gameObject.transform.position;
        minion.GetComponent<PlayerMinion>().SetMinionData(skeletonData);

        Destroy(remains.gameObject);
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
        // eliminar los recursos utilizados en la invocacion
        GameObject minion = Instantiate(playerMinion);

        if (summon == SummonName.SummonA)
        {
            Instantiate(invocationAModel, minion.transform);
            minion.GetComponent<PlayerMinion>().SetMinionData(invocationA);
        }
        if (summon == SummonName.SummonB)
        {
            Instantiate(invocationBModel, minion.transform);
            minion.GetComponent<PlayerMinion>().SetMinionData(invocationB);
        }

        minion.transform.position = remains.transform.position;

        List<ItemType> itemsToRemove = Dictionaries.SummonByMaterials[summon].Item2; 
        // tipos de items que tengo que eliminar de la lista
        
        foreach(ItemType item in itemsToRemove) inventory.RemoveItemByType(item);

        Destroy(remains.transform.gameObject);

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