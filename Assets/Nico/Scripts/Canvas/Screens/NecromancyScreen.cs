using System;
using UnityEngine;


// la pantalla tiene que saber que minion es el cadaver?

public class NecromancyScreen : MonoBehaviour
{
    public static event Action OnButtonPressed_Resurrect;
    public static event Action OnButtonPressed_UseSkeleton;
    public static event Action OnButtonPressed_OpenRitualOptions;
    public static event Action OnButtonPressed_Disect;
    public static event Action OnButtonPressed_CloseMenu;


    
    private RitualPanel ritualPanel;



    private void Awake()
    {
        ritualPanel = GetComponentInChildren<RitualPanel>(includeInactive:true);
    }
    private void OnEnable()
    {
        ritualPanel.OnRitualPerformed += CloseMenu;
    }
    private void OnDisable()
    {
        ritualPanel.OnRitualPerformed -= CloseMenu;
    }


    public void Resurrect()
    {
        OnButtonPressed_Resurrect?.Invoke();
        CloseMenu();
    }
    public void UseSkeleton()
    {
        OnButtonPressed_UseSkeleton?.Invoke();
        CloseMenu();
    }
    public void OpenRitualOptions() 
    {
        OnButtonPressed_OpenRitualOptions?.Invoke();
        ritualPanel.gameObject.SetActive(true);
    }
    public void Disect()
    {
        OnButtonPressed_Disect?.Invoke();
        CloseMenu();
    }
    public void CloseMenu()
    {
        ritualPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);

        OnButtonPressed_CloseMenu?.Invoke();
    }

}
