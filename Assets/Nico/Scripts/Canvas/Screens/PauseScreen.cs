using System;
using UnityEngine;




public class PauseScreen : MonoBehaviour
{
    #region Events
    public event Action OnButtonPressed_ResumeGame;
    #endregion




    [Header("Screens")]
    [SerializeField] private GameObject pauseScreen;

    [Header("Panels")]
    [SerializeField] private GameObject blackPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject optionsPanel;






    

    public void ResumeGame()
    {
        OnButtonPressed_ResumeGame?.Invoke();

        pauseScreen.SetActive(false);
        inventoryPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        optionsPanel.SetActive(false);

    }
    public void OpenOptions()
    {
        inventoryPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }


}
