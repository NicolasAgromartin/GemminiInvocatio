using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class NecromancyScreen : MonoBehaviour
{
    private Remains remains;

    public event Action OnRemainsCanvasClosed;


    [Header("Panels")]
    [SerializeField] private GameObject invocationPanel;

    [Header("Prefabs")]
    [SerializeField] private GameObject invocationBoxPrefab;

    private Necromancy necromancy;


    private void Awake()
    {
        necromancy = FindAnyObjectByType<Player>().GetComponent<Necromancy>();

        invocationPanel = transform.Find("InvocationPanel").gameObject;
    }

    public void OpenMenu(Remains remains)
    {
        gameObject.SetActive(true);
        this.remains = remains;
        CursorManager.EnableCursor();
    }
    public void CloseMenu()
    {
        invocationPanel.SetActive(false);
        this.gameObject.SetActive(false); 
        CursorManager.DisableCursor();
        // borrar summonboxes
        foreach(GameObject box in instantiatedBoxes)
        {
            Destroy(box);
        }
        instantiatedBoxes.Clear();
        OnRemainsCanvasClosed?.Invoke();
    }




    private List<GameObject> instantiatedBoxes = new();
    public void Resurrect()
    {
        necromancy.Resurrect(remains);
        CloseMenu();
    }
    public void OpenRitualOptions() 
    {
        if (invocationPanel.activeSelf) return;
        invocationPanel.SetActive(true);

        List<SummonName> possibleSummons = necromancy.GetPossibleSummons(remains);

        foreach (SummonName summon in possibleSummons)
        {
            GameObject ritual = Instantiate(invocationBoxPrefab, invocationPanel.transform);
            ritual.GetComponentInChildren<TMP_Text>().text = summon.ToString();
            ritual.GetComponent<Button>().onClick.AddListener(() => necromancy.Summon(summon, remains)); 
            ritual.GetComponent<Button>().onClick.AddListener(() => CloseMenu());
            instantiatedBoxes.Add(ritual);
        }
    }
    public void UseSkeleton()
    {
        necromancy.UseSkeleton(remains);
        CloseMenu();
    }
    public void Disect()
    {
        Debug.Log(remains.name);
        necromancy.Disect(remains.gameObject);
        CloseMenu();
    }




}
