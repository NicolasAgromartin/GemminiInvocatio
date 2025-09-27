using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;





public static class NecromancySystem
{
    public static event Action<GameObject> OnUnitResurrected;


    public static void Resurrect(Remains remains)
    {
        GameObject minion = remains.gameObject;

        minion.tag = "PlayerMinion";
        minion.name = "PlayerMinion - " + $"{remains.Data.name}";

        PlayerMinion playerMinion = minion.AddComponent<PlayerMinion>();
        playerMinion.SetMinionData(remains.Data);
        
        OnUnitResurrected?.Invoke(remains.gameObject);
    }

    public static void Disect(GameObject unit)
    {
        // se puede extraer materiales de las unidades que tengamos como aliadas, 
        // haciendoles perder vida o matandolas
        Debug.Log($"{unit} ready to disect");
    }

    public static void RecieveMaterials(List<Remains> materials)
    {
        // instanciar el prefab de la unidad invocada
        // 
    }
}


