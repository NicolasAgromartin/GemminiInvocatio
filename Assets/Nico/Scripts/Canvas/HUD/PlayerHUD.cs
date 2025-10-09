using TMPro;
using System.Collections.Generic;
using UnityEngine;





public class PlayerHUD : MonoBehaviour
{
    private HealthBar healthBar;
    private Lives lives;
    private Target target;
    private MinionsPanel minionsPanel;
    private TMP_Text currentState;

    private Player player;




    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        healthBar = GetComponentInChildren<HealthBar>();
        lives = GetComponentInChildren<Lives>();
        target = GetComponentInChildren<Target>();
        minionsPanel = GetComponentInChildren<MinionsPanel>();
        currentState = transform.Find("CurrentState").GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
        player.OnDamageRecieved += ChangeHealth;
        player.OnHealthIncreased += ChangeHealth;
        player.OnDeath += ChangeLives;

        PlayerStateMachine.OnStateChange += UpdateCurrentState;
    }
    private void OnDisable()
    {
        player.OnDamageRecieved -= ChangeHealth;
        player.OnHealthIncreased -= ChangeHealth;
        player.OnDeath -= ChangeLives;

        PlayerStateMachine.OnStateChange -= UpdateCurrentState;
    }





    #region Health
    private void ChangeHealth(Unit player, int newHealth)
    {
        healthBar.ChangeHealth(newHealth);
    }
    #endregion

    #region Lives
    private void ChangeLives(Unit player)
    {
        lives.ChangeLives(player.GetComponent<Player>().Lives);
    }
    #endregion

    #region Minions Panel
    private void UpdateMinions(List<PlayerMinion> minions)
    {

    }
    #endregion

    #region StateManager
    private void UpdateCurrentState(BaseState currentState) => this.currentState.text = currentState.ToString();
    #endregion

}

