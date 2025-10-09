using System;
using System.Collections.Generic;
using UnityEngine;


public class Inventory
{
    public event Action<ItemType> OnItemListChanged;
    public static event Action<Dictionary<ItemType, List<Item>>> OnItemsUpdated;

    public static Dictionary<ItemType, List<Item>> Items { get; private set; }

    

    public Inventory()
    {
        Items = new();

        foreach(ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            Items.Add(type, new());
        }
    }



    public void AddItem(Item item)
    {
        Items[item.Type].Add(item);
        OnItemListChanged?.Invoke(item.Type);

        OnItemsUpdated?.Invoke(Items);
    }
    public void RemoveItem(ItemType type, Item item)
    {
        Items[item.Type].Remove(item);
        OnItemListChanged?.Invoke(item.Type);
        
        OnItemsUpdated?.Invoke(Items);
    }
    public void RemoveItemByType(ItemType type)
    {
        if (Items[type].Count > 0)
        {
            Items[type].RemoveAt(0);
            OnItemListChanged?.Invoke(type);
            
            OnItemsUpdated?.Invoke(Items);
        }
        else
        {
            Debug.Log($"There aint no more items to remove of the type {type}");
        }
    }
    public static List<Item> GetItems(ItemType type) => Items[type];
    public Dictionary<ItemType, List<Item>> GetAllItems() => Items;
    public Item GetKey()
    {
        if (Items[ItemType.KeyItem].Count  > 0)
        {
            Item keyToReturn = Items[ItemType.KeyItem][0];
            RemoveItem(ItemType.KeyItem, keyToReturn);
            return keyToReturn;
        }
        else
        {
            return null;
        }
    }




}
