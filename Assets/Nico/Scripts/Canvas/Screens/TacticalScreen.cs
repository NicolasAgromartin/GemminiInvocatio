using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class TacticalScreen : MonoBehaviour
{
    public static event Action OnButtonPressed_Move;
    public static event Action OnButtonPressed_Return;
    public static event Action OnButtonPressed_ChangeTarget;

    public static event Action OnMinionPanelClosed;

    private GameObject minionTacticsUI;




    private void Awake()
    {
        minionTacticsUI = transform.Find("MinionTactics").gameObject;
    }
    private void OnEnable()
    {
        // cuando habilito la pantalla de tactics, espero por la seleccion de un minion
        MinionOwner.OnMinionSelected += ShowMinionTactics;
    }
    private void OnDisable()
    {
        minionTacticsUI.SetActive(false);
        MinionOwner.OnMinionSelected -= ShowMinionTactics;
    }



    // cada vez que clickeo sobre un minion se ejecuta una suscripcion en los btones
    // tengo que hacer que sea una unica suscripcion por minion
    private PlayerMinion currentSelectedMinion = null;
    public void ShowMinionTactics(PlayerMinion selectedMinion)
    {
        if (selectedMinion == null || currentSelectedMinion == selectedMinion) return;

        currentSelectedMinion = selectedMinion;
        minionTacticsUI.SetActive(true);
        minionTacticsUI.GetComponentInChildren<TMP_Text>().text = selectedMinion.name;

        RemoveButtonsListener();
        SetMinionTacticsButtonsAction();
    }
    private void SetMinionTacticsButtonsAction()
    {
        minionTacticsUI.transform.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(
            () => { HideMinionTactics(); OnButtonPressed_Return?.Invoke(); });

        minionTacticsUI.transform.Find("MoveMinionButton").GetComponent<Button>().onClick.AddListener(
            () => { HideMinionTactics(); OnButtonPressed_Move?.Invoke(); });

        minionTacticsUI.transform.Find("ChangeTargetButton").GetComponent<Button>().onClick.AddListener(
            () => { HideMinionTactics(); OnButtonPressed_ChangeTarget?.Invoke(); });
    }
    public void HideMinionTactics()
    {
        Debug.Log("Button pressed");
        currentSelectedMinion = null;
        RemoveButtonsListener();

        minionTacticsUI.SetActive(false);
        OnMinionPanelClosed?.Invoke();
    }

    private void RemoveButtonsListener()
    {
        minionTacticsUI.transform.Find("ReturnButton").GetComponent<Button>().onClick.RemoveAllListeners();
        minionTacticsUI.transform.Find("MoveMinionButton").GetComponent<Button>().onClick.RemoveAllListeners();
        minionTacticsUI.transform.Find("ChangeTargetButton").GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
