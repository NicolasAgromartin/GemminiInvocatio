using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RitualPanel : MonoBehaviour
{
    public static event Action OnPanelOpened;
    public static event Action OnPanelClosed;
    public static event Action<SummonName> OnButtonPressed_Summon;
    public event Action OnRitualPerformed;

    [Header("Prefab")]
    [SerializeField] private GameObject ritualBox;
    private readonly List<GameObject> instantiatedBoxes = new();





    private void OnEnable()
    {
        Necromancy.OnRitualsObtained += ShowPossibleRituals;
        OnPanelOpened?.Invoke();
    }
    private void OnDisable()
    {
        Necromancy.OnRitualsObtained -= ShowPossibleRituals;
        OnPanelClosed?.Invoke();

        foreach (GameObject box in instantiatedBoxes) Destroy(box);
        instantiatedBoxes.Clear();
    }

    private void ShowPossibleRituals(List<SummonName> possibleSummons)
    {
        foreach (SummonName possibleSummon in possibleSummons)
        {
            GameObject ritual = Instantiate(ritualBox, transform);
            ritual.GetComponentInChildren<TMP_Text>().text = possibleSummon.ToString();
            ritual.GetComponent<Button>().onClick.AddListener(() => Summon(possibleSummon));
            //ritual.GetComponent<Button>().onClick.AddListener(() => CloseMenu());
            instantiatedBoxes.Add(ritual);
        }
    }
    public void Summon(SummonName summonName)
    {
        OnButtonPressed_Summon?.Invoke(summonName);
        OnRitualPerformed?.Invoke();
    }
}
