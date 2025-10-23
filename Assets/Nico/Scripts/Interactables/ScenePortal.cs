using System;
using System.Collections.Generic;
using UnityEngine;
using static SceneLoader;



public class ScenePortal : MonoBehaviour, IInteractable
{
    public event Action<ScenePortal> OnPortalInteracted;

    public List<SceneNames> ScenesToGo => scenesToGo;
    [SerializeField] private List<SceneNames> scenesToGo;



    public void Interact(GameObject interactor)
    {
        OnPortalInteracted?.Invoke(this);
    }


}
