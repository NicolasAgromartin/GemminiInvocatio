using UnityEngine;
using UnityEngine.UI;

public class BlackScreenPanel : MonoBehaviour
{
    public Material material;
    private Image backgroundImage;



    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        PauseManager.OnPauseToggled += PauseResumeGame;
        //Player.OnLifeLost += EnableBlackScreen;
    }
    private void OnDisable()
    {
        PauseManager.OnPauseToggled -= PauseResumeGame;
        //Player.OnLifeLost -= EnableBlackScreen;
    }
    void Update()
    {
        material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }



    private void PauseResumeGame(bool isGamePaused)
    {
        if (isGamePaused)
        {
            EnableBlackScreen();
        }
        else
        {
            DisableBlackScreen();
        }
    }
    private void EnableBlackScreen()
    {
        backgroundImage.enabled = true;
    }
    private void DisableBlackScreen()
    {
        backgroundImage.enabled = false;
    }
}
