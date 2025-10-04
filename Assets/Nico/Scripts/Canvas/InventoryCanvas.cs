using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;




public class InventoryCanvas : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject itemIndicatorPrefab;
    [SerializeField] private TMP_Text potionsCounter;

    [SerializeField] private GameObject inventoryPanel;

    private Dictionary<ItemType, GameObject> itemsIndicator = new();
    private Inventory inventory;








    public void SetInventory(Inventory inventory)
    {
        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            GameObject indicator = Instantiate(itemIndicatorPrefab, inventoryPanel.transform);
            itemsIndicator.Add(type, indicator);
            indicator.transform.Find("ItemName").GetComponent<TMP_Text>().text = type.ToString();
            itemsIndicator[type].transform.Find("ItemCount").GetComponent<TMP_Text>().text = 0.ToString();
        }

        this.inventory = inventory;

        inventory.OnItemListChanged += UpdateInventory;
    } 




    private void UpdateInventory(ItemType itemType)
    {
        itemsIndicator[itemType].transform.Find("ItemCount").GetComponent<TMP_Text>().text = inventory.GetItems(itemType).Count.ToString();

        IncreasePotionsCounter(inventory.GetItems(ItemType.Potion).Count);
    }


    public void IncreasePotionsCounter(int amount)
    {
        potionsCounter.text = amount.ToString();
    }
}
