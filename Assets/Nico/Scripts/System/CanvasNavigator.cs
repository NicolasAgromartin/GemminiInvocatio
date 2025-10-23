using UnityEngine;
using System;




public class CanvasNavigator : MonoBehaviour
{
    #region Events
    public event Action OnButtonPressed_Options;
    public event Action OnButtonPressed_Instructions;
    #endregion

    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject optionsPanel;

    private AudioSource audioSource;




    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CursorManager.EnableCursor();
    }


    public void Options()
    {
        OnButtonPressed_Options?.Invoke();
        optionsPanel.SetActive(true);
    }
    public void Instructions()
    {
        OnButtonPressed_Instructions?.Invoke();
        instructionsPanel.SetActive(true);
    }
    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
        audioSource.Play();
    }
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        audioSource.Play();
    }
}
