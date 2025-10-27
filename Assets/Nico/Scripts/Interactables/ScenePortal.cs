using System;
using System.Collections.Generic;
using UnityEngine;
using static SceneLoader;



public class ScenePortal : MonoBehaviour, IInteractable
{
    public event Action<ScenePortal> OnPortalInteracted;

    [SerializeField] private int id = 0;
    [SerializeField] private List<SceneNames> scenesToGo;



    public void Interact(GameObject interactor)
    {

        if (id == 0) Debug.Log("This door desnt need a key");
        else
        {
            if (interactor.GetComponent<Player>().FindKey(id))
            {
                OnPortalInteracted?.Invoke(this);
                Debug.Log("Can be opened");
                // ruido a puerta que se abre?
            }
            else
            {
                Debug.Log("Key needed");
                // ruido de puerta cerrada
                // carten en pantalla?
            }
        }
    }




    public List<SceneNames> ScenesToGo => scenesToGo;
}
