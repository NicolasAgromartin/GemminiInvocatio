using UnityEngine;



public class ScreenManager : MonoBehaviour
{
    private PauseScreen pauseScreen;
    private BlackScreen blackScreen;
    private DefeatScreen defeatScreen;
    private TacticalScreen tacticalScreen;
    private NecromancyScreen necromancyScreen;

    private Player player;




    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        pauseScreen = GetComponentInChildren<PauseScreen>(includeInactive:true);
        blackScreen = GetComponentInChildren<BlackScreen>(includeInactive:true);
        defeatScreen = GetComponentInChildren<DefeatScreen>(includeInactive: true);
        tacticalScreen = GetComponentInChildren<TacticalScreen>(includeInactive: true);
        necromancyScreen = GetComponentInChildren<NecromancyScreen>(includeInactive: true);
    }
    private void OnEnable()
    {
        PauseManager.OnPauseToggled += TogglePauseScreen;

        PlayerStateMachine.OnStateChange += HandleStateChange;
    }
    private void OnDisable()
    {
        PauseManager.OnPauseToggled -= TogglePauseScreen;

        PlayerStateMachine.OnStateChange -= HandleStateChange;
    }





    private void ShowDefeatScreen()
    {
        defeatScreen.gameObject.SetActive(true);
    }
    private void HideDefeatScreen()
    {
        defeatScreen.gameObject.SetActive(false);
    }


    private void SuscribeToBlackScreen()
    {

    }


    private void TogglePauseScreen(bool isGamePaused)
    {
        if (isGamePaused)
        {
            pauseScreen.gameObject.SetActive(true);
        }
        else
        {
            pauseScreen.gameObject.SetActive(false);
        }
    }

    private void HandleStateChange(BaseState currentState)
    {
        switch(currentState)
        {
            case PlayerTacticsState:
                tacticalScreen.gameObject.SetActive(true);
                necromancyScreen.gameObject.SetActive(false);
                break;
            case PlayerInteractState:
                necromancyScreen.gameObject.SetActive(true);
                tacticalScreen.gameObject.SetActive(false);
                break;
            case PlayerDeadState:
                blackScreen.gameObject.SetActive(true);
                defeatScreen.gameObject.SetActive(true);

                tacticalScreen.gameObject.SetActive(false);
                necromancyScreen.gameObject.SetActive(false);
                break;
            default:
                tacticalScreen.gameObject.SetActive(false);
                necromancyScreen.gameObject.SetActive(false);
                break;
        }
    }
}
