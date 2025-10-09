using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject itemBox;


    private readonly Dictionary<ItemType, GameObject> itemBoxes = new();


    private void Awake()
    {
        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            GameObject item = Instantiate(itemBox, transform);

            itemBoxes.Add(type, item);
            item.transform.Find("ItemName").GetComponent<TMP_Text>().text = type.ToString();
            item.transform.Find("ItemCount").GetComponent<TMP_Text>().text = 0.ToString();
        }
    }
    private void OnEnable()
    {
        foreach (List<Item> itemsList in Inventory.Items.Values)
        {
            foreach (Item item in itemsList)
            {
                itemBoxes[item.Type].transform.Find("ItemCount").GetComponent<TMP_Text>().text = itemsList.Count.ToString();
            }
        }
    }

}
