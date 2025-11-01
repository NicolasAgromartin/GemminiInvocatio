using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SceneLoader;



public class ScenePortal : MonoBehaviour, IInteractable
{
    public event Action<ScenePortal> OnPortalInteracted;

    [SerializeField] private int id = 0;
    [SerializeField] private List<SceneNames> scenesToGo;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    private void OnEnable()
    {
        Inventory.OnItemAdded += PlayerKeyCheck;
    }
    private void OnDisable()
    {
        Inventory.OnItemAdded -= PlayerKeyCheck;
    }
    private void Start()
    {
        if(id != 0)
        {
            rend.material = new(rend.material);
            rend.material.SetColor("_Color", Color.red);
        }
    }

    private void PlayerKeyCheck(ItemType itemType)
    {
        if(itemType == ItemType.KeyItem)
        {
            Renderer rend = GetComponent<Renderer>();
            // cambiar el color del material a verde

            rend.material = new(rend.material);

            rend.material.SetColor("_Color", Color.green);
        }
    }

    public void Interact(GameObject interactor)
    {

        if (id == 0)
        {
            Debug.Log("This door desnt need a key");
            OnPortalInteracted?.Invoke(this);
        }
        else
        {
            if (interactor.GetComponent<Player>().FindKey(id))
            {
                Debug.Log("Can be opened");
                OnPortalInteracted?.Invoke(this);
            }
            else
            {
                Debug.Log("Key needed");
            }
        }
    }




    public List<SceneNames> ScenesToGo => scenesToGo;
}
