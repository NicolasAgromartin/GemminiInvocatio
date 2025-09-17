using System;
using System.Collections.Generic;
using UnityEngine;


public class Inventory
{
    public event Action<ItemType> OnItemListChanged;


    private Dictionary<ItemType, List<Item>> items;


    public Inventory()
    {
        items = new();

        foreach(ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            items.Add(type, new());
        }
    }



    public void AddItem(Item item)
    {
        //Debug.Log($"Aded {item.Type} to the list");
        items[item.Type].Add(item);
        OnItemListChanged?.Invoke(item.Type);
    }
    public void RemoveItem(ItemType type, Item item)
    {
        items[item.Type].Remove(item);
        OnItemListChanged?.Invoke(item.Type);
    }
    public void RemoveItemByType(ItemType type)
    {
        if (items[type].Count > 0)
        {
            items[type].RemoveAt(0);
            OnItemListChanged?.Invoke(type);
        }
        else
        {
            Debug.Log($"There aint no more items to remove of the type {type}");
        }
    }
    public List<Item> GetItems(ItemType type) => items[type];
    public Dictionary<ItemType, List<Item>> GetAllItems() => items;
    public Item GetKey()
    {
        if (items[ItemType.KeyItem].Count  > 0)
        {
            Item keyToReturn = items[ItemType.KeyItem][0];
            RemoveItem(ItemType.KeyItem, keyToReturn);
            return keyToReturn;
        }
        else
        {
            return null;
        }
    }
}
