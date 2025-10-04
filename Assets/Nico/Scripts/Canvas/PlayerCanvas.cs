using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class PlayerCanvas : MonoBehaviour
{
    public event Action OnReturnOrder;
    public event Action OnTargetChangeOrder;
    public event Action OnMoveOrder;



    [Header("Components")]
    [Header("Minions")]
    [SerializeField] private GameObject minionsDisplay;
    [SerializeField] private GameObject minionUiPrefab;

    [Header("Header")]
    [SerializeField] private TMP_Text currentHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject lives;
    [SerializeField] private TMP_Text focusedTarget;

    [Header("Minion Tactics")]
    [SerializeField] private GameObject minionTacticsUI;

    [Header("Right Side")]
    [SerializeField] private TMP_Text potionsCounter;

    [Header("Black Screen")]
    [SerializeField] private GameObject blackPanel;
    [SerializeField] private GameObject pauseScreen;



    private Dictionary<PlayerMinion, GameObject> playerMinionBoxes = new();
    private Player player;





    #region Life Cykle
    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void OnEnable()
    {
        TacticsSystem.OnEnemySelected += ChangeFocusedTarget;
        TacticsSystem.OnEnemyUnselected += RemoveFocusedTarget;

        PauseManager.OnPauseToggled += PauseResumeGame;

        player.OnMinionsUpdated += UpdateMinions;
        player.OnHealthChanged += ChangeHealth;
        player.OnLivesChanged += ChangeLives;
    }
    private void OnDisable()
    {
        TacticsSystem.OnEnemySelected -= ChangeFocusedTarget;
        TacticsSystem.OnEnemyUnselected -= RemoveFocusedTarget;

        PauseManager.OnPauseToggled -= PauseResumeGame;

        player.OnMinionsUpdated -= UpdateMinions;
        player.OnHealthChanged -= ChangeHealth;
        player.OnLivesChanged -= ChangeLives;
    }
    #endregion






    #region Minions UI
    private void UpdateMinions(List<PlayerMinion> minions)
    {

        foreach(PlayerMinion minion in minions)
        {
            if(minion == null) continue;    
            if (playerMinionBoxes.ContainsKey(minion)) continue;

            GameObject newMinionUI = Instantiate(minionUiPrefab, minionsDisplay.transform);
            newMinionUI.GetComponentInChildren<TMP_Text>().text = minion.name;
            newMinionUI.transform.Find("Stats/AttackValue").GetComponent<TMP_Text>().text = "Atk : " + minion.Stats.Attack.ToString();

            playerMinionBoxes.Add(minion, newMinionUI);
            SuscribeToMinionEvents(minion);
        }

    }
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

    private void UpdateMinionHealth(Unit minion, int newHealth)
    {
        StartCoroutine(ChangeHealthBar(playerMinionBoxes[minion.GetComponent<PlayerMinion>()].transform.Find("Health/HealthBar").GetComponent<Image>(), newHealth));
    }
    private void RemoveMinionFromList(Unit minion)
    {
        PlayerMinion deadMinion = minion.GetComponent<PlayerMinion>();

        UnsuscribeToMinionEvents(deadMinion);

        playerMinionBoxes.TryGetValue(deadMinion, out GameObject minionBox);
        playerMinionBoxes.Remove(deadMinion);
        minionBox.SetActive(false);
    }
    #endregion





    #region Minion Tactics
    public void ShowMinionTactics(GameObject selectedMinion)
    {
        minionTacticsUI.SetActive(true);
        minionTacticsUI.GetComponentInChildren<TMP_Text>().text = selectedMinion.name;

        SetMinionTacticsButtonsAction();
    }
    private void SetMinionTacticsButtonsAction()
    {
        minionTacticsUI.transform.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(
            () => { HideMinionTactics(); OnReturnOrder?.Invoke(); } );

        minionTacticsUI.transform.Find("MoveMinionButton").GetComponent<Button>().onClick.AddListener(
            ()=> { HideMinionTactics(); OnMoveOrder?.Invoke(); });

        minionTacticsUI.transform.Find("ChangeTargetButton").GetComponent<Button>().onClick.AddListener(
            ()=> { HideMinionTactics(); OnTargetChangeOrder?.Invoke(); });
    }
    public void HideMinionTactics()
    {
        minionTacticsUI.transform.Find("ReturnButton").GetComponent<Button>().onClick.RemoveAllListeners();
        minionTacticsUI.transform.Find("MoveMinionButton").GetComponent<Button>().onClick.RemoveAllListeners();
        minionTacticsUI.transform.Find("ChangeTargetButton").GetComponent<Button>().onClick.RemoveAllListeners();

        minionTacticsUI.SetActive(false);
    }
    #endregion



    #region Header
    private void ChangeHealth(int newHealth)
    {
        //Debug.Log(newHealth);
        currentHealth.text = $"{newHealth}% HP";
        StartCoroutine(ChangeHealthBar(healthBar, newHealth));
    }
    private void ChangeLives(int newLives)
    {
        lives.GetComponentInChildren<TMP_Text>().text = newLives.ToString();
    }
    public void ChangeFocusedTarget(GameObject newTarget) 
    { 
        if(newTarget == null)
        {
            focusedTarget.text = string.Empty;
        }
        else
        {
            focusedTarget.text = newTarget.name;
        }
    }
    public void RemoveFocusedTarget()
    {
        focusedTarget.text = string.Empty;
    }
    #endregion



    private void PauseResumeGame(bool isGamePaused)
    {
        if (isGamePaused)
        {
            blackPanel.SetActive(true);
            pauseScreen.SetActive(true);
        }
        else
        {
            blackPanel.SetActive(false);
            pauseScreen.SetActive(false);
        }
    }


    private IEnumerator ChangeHealthBar(Image healthBar, int newHealth)
    {
        if (newHealth > 100) yield return null;
        else
        {
            float target = newHealth / 100f;
            float speed = 0.01f;

            while (!Mathf.Approximately(healthBar.fillAmount, target))
            {
                healthBar.fillAmount = Mathf.MoveTowards(
                    healthBar.fillAmount,
                    target,
                    speed
                );

                yield return null;
            }

        }
    }

}
