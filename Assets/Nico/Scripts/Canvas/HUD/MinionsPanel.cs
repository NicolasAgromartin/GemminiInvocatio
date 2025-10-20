using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinionsPanel : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject minionBox;
    private readonly Dictionary<PlayerMinion, GameObject> minionBoxes = new();


    private void OnEnable()
    {
        MinionOwner.OnMinionsUpdated += UpdateMinions;
    }
    private void OnDisable()
    {
        MinionOwner.OnMinionsUpdated -= UpdateMinions;
    }





    private void UpdateMinions(List<PlayerMinion> minions)
    {
        foreach (PlayerMinion minion in minions)
        {
            if (minion == null) continue;
            if (minionBoxes.ContainsKey(minion)) continue;

            GameObject newMinionUI = Instantiate(minionBox, transform);

            // aca agrego el icono de minion a la ui
            newMinionUI.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = minion.GetFiendIcon();
            newMinionUI.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = minion.GetFiendName();

            minionBoxes.Add(minion, newMinionUI);
            SuscribeToMinionEvents(minion);
        }
    }


    private void UpdateMinionHealth(Unit minion, int newHealth)
    {
        minionBoxes[minion.gameObject.GetComponent<PlayerMinion>()].GetComponentInChildren<HealthBar>().ChangeHealth(newHealth, minion.Stats.MaxHealth);
    }
    private void RemoveMinionFromList(Unit minion)
    {
        PlayerMinion deadMinion = minion.GetComponent<PlayerMinion>();

        UnsuscribeToMinionEvents(deadMinion);

        minionBoxes.TryGetValue(deadMinion, out GameObject minionBox);
        minionBoxes.Remove(deadMinion);
        minionBox.SetActive(false);
    }





    #region Minion Events
    private void SuscribeToMinionEvents(PlayerMinion minion)
    {
        minion.OnDamageRecieved += UpdateMinionHealth;
        minion.OnDeath += RemoveMinionFromList;
    }
    private void UnsuscribeToMinionEvents(PlayerMinion minion)
    {
        minion.OnDamageRecieved -= UpdateMinionHealth;
        minion.OnDeath -= RemoveMinionFromList;
    }
    #endregion
}
