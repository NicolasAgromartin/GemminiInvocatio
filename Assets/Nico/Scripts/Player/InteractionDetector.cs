using System;
using UnityEngine;






public class InteractionDetector : MonoBehaviour
{
    public static Action<GameObject> OnInteractionDetected;
    public static Action<GameObject> OnInteractionLost;

    private GameObject detected;


    private void OnEnable()
    {
        Inventory.OnItemAdded += OnItemPicked;
        PlayerStateMachine.OnStateChange += StateHandler;
    }
    private void OnDisable()
    {
        Inventory.OnItemAdded -= OnItemPicked;
        PlayerStateMachine.OnStateChange -= StateHandler;
    }




    private void OnTriggerEnter(Collider other)
    {
        detected = other.gameObject;

        Debug.Log(detected);

        if (detected.CompareTag("Item") || detected.CompareTag("ScenePortal") || detected.CompareTag("Remains"))
        {
            OnInteractionDetected?.Invoke(detected);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        detected = other.gameObject;

        if (detected.CompareTag("Item") || detected.CompareTag("ScenePortal") || detected.CompareTag("Remains"))
        {
            OnInteractionLost?.Invoke(null);
        }
    }

    private void OnItemPicked(ItemType itemType)
    {
        Pickable picked = detected.GetComponent<Pickable>();

        if (picked == null) return;

        if (detected != null && itemType == picked.Data.type)
        {
            detected = null;
            OnInteractionLost?.Invoke(null);
        }
    }

    private void StateHandler(BaseState state)
    {
        if (state is PlayerInteractState)
        {
            detected = null;
            OnInteractionLost?.Invoke(null);
        }
        else return;
    }
}
