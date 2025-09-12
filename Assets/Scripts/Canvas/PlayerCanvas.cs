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




    private List<GameObject> activeMinionsList = new();
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
            if (activeMinionsList.Contains(minion.gameObject)) continue;

            GameObject newMinionUI = Instantiate(minionUiPrefab, minionsDisplay.transform);
            newMinionUI.GetComponentInChildren<TMP_Text>().text = minion.gameObject.name;

            activeMinionsList.Add(minion.gameObject);
        }
    }
    #endregion





    #region Minion Tactics
    public void ShowMinionTactics(GameObject selectedMinion)
    {
        GameObject minionToShow = activeMinionsList.Find(minion => selectedMinion == minion);

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
        currentHealth.text = $"{newHealth.ToString()}% HP";

        //if (newHealth > 100) healthBar.fillAmount = 1;
        //else healthBar.fillAmount = newHealth / 100f;
        StartCoroutine(ChangeHealthBar(newHealth));
    }
    private IEnumerator ChangeHealthBar(int newHealth)
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


}
