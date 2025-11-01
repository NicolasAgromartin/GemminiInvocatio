using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private List<Item_SO> items;
    [Header("Prefab")]
    [SerializeField] private GameObject itemBox;
    [SerializeField] private GameObject contentArea;

    private readonly Dictionary<ItemType, GameObject> itemBoxes = new();


    private void Awake()
    {
        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            if (type != ItemType.Potion && type != ItemType.KeyItem) continue;

            GameObject item = Instantiate(itemBox, contentArea.transform);

            itemBoxes.Add(type, item);
            item.transform.Find("ItemName").GetComponent<TMP_Text>().text = type.ToString();
        }
    }
    private void OnEnable()
    {
        foreach (List<Item> itemsList in Inventory.Items.Values)
        {
            foreach (Item item in itemsList)
            {

                if (item.Data.type != ItemType.Potion && item.Data.type != ItemType.KeyItem) continue;

                itemBoxes[item.Type].transform.Find("ItemCount").GetComponent<TMP_Text>().text = itemsList.Count.ToString();
                itemBoxes[item.Type].transform.Find("ItemDescription").GetComponent<TMP_Text>().text = itemsList.First().Description;
                itemBoxes[item.Type].transform.Find("ItemIcon").GetComponent<Image>().sprite = itemsList.First().Icon;
            }
        }
    }

}
