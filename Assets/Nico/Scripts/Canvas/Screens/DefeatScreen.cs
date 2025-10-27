using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;




public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject defeatUI;
    [SerializeField] private Image blackScreen;





    private void Awake()
    {
        HideUI();
    }
    private void OnEnable()
    {
        StartCoroutine(EntranceSequence());
        RespawnManager.OnPlayerRespawned += HandleRespawn;
    }
    private void OnDisable()
    {
        RespawnManager.OnPlayerRespawned -= HandleRespawn;
    }








    private IEnumerator EntranceSequence()
    {
        yield return new WaitForSeconds(2f);

        blackScreen.gameObject.SetActive(true);
        blackScreen.color = new(0f, 0f, 0f, 0f);

        yield return StartCoroutine(UIEffects.FadeIn(blackScreen, .5f));

        // if player
        if(FindAnyObjectByType<Player>().Lives == 0)
        {
            ShowGameOver();
        }
        else
        {
            //ShowButtons();
            ShowDefeat();
        }
        CursorManager.EnableCursor();
    }

    private void ShowGameOver()
    {
        gameOverUI.SetActive(true);
    }
    private void ShowDefeat()
    {
        defeatUI.SetActive(true);
    }


    private void HandleRespawn()
    {
        StartCoroutine(ExitSequence());
    }
    private IEnumerator ExitSequence()
    {

        HideUI();
        yield return new WaitForSeconds(1f); // o cambiar la suscripcion del respawn manager --> a suscribirme al player restored

        yield return StartCoroutine(UIEffects.FadeOut(blackScreen, 1f));

        Debug.Log("esta corrutina termino?");
        gameObject.SetActive(false);
    }
    private void HideUI()
    {
        gameOverUI.SetActive(false);
        defeatUI.SetActive(false);
        CursorManager.DisableCursor();
    }










    #region UI Buttons
    public void Retry()
    {
        RespawnManager.Instance.RespawnPlayer();
    }
    #endregion
}
