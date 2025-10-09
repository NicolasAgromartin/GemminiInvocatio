using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            newMinionUI.GetComponentInChildren<TMP_Text>().text = minion.name;
            newMinionUI.transform.Find("Stats/AttackValue").GetComponent<TMP_Text>().text = "Atk : " + minion.Stats.Attack.ToString();

            minionBoxes.Add(minion, newMinionUI);
            SuscribeToMinionEvents(minion);
        }
    }


    private void UpdateMinionHealth(Unit minion, int newHealth)
    {
        //StartCoroutine(ChangeHealthBar(playerMinionBoxes[minion.GetComponent<PlayerMinion>()].transform.Find("Health/HealthBar").GetComponent<Image>(), newHealth));
        Debug.Log($"{minion.name} reduce life to {newHealth}");
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
