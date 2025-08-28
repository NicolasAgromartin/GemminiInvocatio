using System;
using UnityEngine;

public class RemainsCanvas : MonoBehaviour
{
    private Remains remains;

    public event Action OnRemainsCanvasClosed;


    [Header("Panels")]
    [SerializeField] private GameObject invocationPanel;
    [SerializeField] private GameObject inventoryPanel;

    public void OpenMenu(Remains remains)
    {
        gameObject.SetActive(true);
        this.remains = remains;
        CursorManager.EnableCursor();
    }




    #region UI Buttons
    public void Resurrect()
    {
        NecromancySystem.Resurrect(remains);
        CloseMenu();
    }
    public void UseAsMaterial()
    {
        invocationPanel.SetActive(true);
        inventoryPanel.SetActive(true);
    }
    public void Disect()
    {
        NecromancySystem.Disect(remains.gameObject);
        CloseMenu();
    }
    public void CloseMenu()
    {
        this.gameObject.SetActive(false);
        CursorManager.DisableCursor();
        OnRemainsCanvasClosed?.Invoke();
    }
    #endregion


    #region Invocation UI

    // mostrar una lista de las invocaciones disponibles
    // cruzar datos del inventario del jugador con el cadaver
    // resaltar las opciones disponibles

    #endregion
}
