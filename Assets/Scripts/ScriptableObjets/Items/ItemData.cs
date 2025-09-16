using UnityEngine;


[System.Serializable]
public abstract class Item_SO : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public GameObject model;
    public Sprite icon;
    public ItemType type;
}


[System.Serializable]
public abstract class UniqueItem_SO: Item_SO
{
    public string Id;
}
