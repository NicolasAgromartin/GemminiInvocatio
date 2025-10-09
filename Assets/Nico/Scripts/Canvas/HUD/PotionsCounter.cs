using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PotionsCounter : MonoBehaviour
{

    private TMP_Text potionsCount;


    private void Awake()
    {
        potionsCount = GetComponentInChildren<TMP_Text>();
    }
    private void OnEnable()
    {
        Inventory.OnItemsUpdated += UpdatePotionCounter;
    }
    private void OnDisable()
    {
        Inventory.OnItemsUpdated += UpdatePotionCounter;
    }


    private void UpdatePotionCounter(Dictionary<ItemType, List<Item>> items)
    {
        potionsCount.text = items[ItemType.Potion].Count.ToString();
    }

}
