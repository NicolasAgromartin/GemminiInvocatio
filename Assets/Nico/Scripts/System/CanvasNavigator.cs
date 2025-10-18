using UnityEngine;
using System;




public class CanvasNavigator : MonoBehaviour
{
    #region Events
    public event Action OnButtonPressed_Options;
    public event Action OnButtonPressed_Instructions;
    #endregion

    [SerializeField] private GameObject instructionsPanel;


    public void Options()
    {
        OnButtonPressed_Options?.Invoke();
    }
    public void Instructions()
    {
        OnButtonPressed_Instructions?.Invoke();
        instructionsPanel.SetActive(true);
    }
    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
    }
}
