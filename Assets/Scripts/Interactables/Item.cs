using UnityEngine;




[System.Serializable]
public class Item
{
    public string ItemName { get; private set; }
    public string Description { get; private set; }
    public Sprite Icon { get; private set; }
    public ItemType Type { get; private set; }



    public Item(Item_SO data)
    {
        ItemName = data.itemName;
        Description = data.description;
        Icon = data.icon;
        Type = data.type;
    }


    public void Use(Player player)
    {
        if(Type == ItemType.Potion) player.IncreaseHealth(10);
    }
}