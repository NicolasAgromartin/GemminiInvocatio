using Unity.VisualScripting;
using UnityEngine;


 


public class Remains : MonoBehaviour, IInteractable, IDisectable
{
    private SphereCollider interactionTrigger;

    private readonly float triggerRadius = 1.5f;
    private readonly Vector3 triggerCenter = new(0f, 0f, 0f);

    public FiendSO Data { get; private set; }



    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        SetInteractionTrigger();
    }
    public void SetRemainsData(FiendSO data) => Data = data;



    public void Interact(GameObject interactor)
    {
        if (interactor.CompareTag("Player"))
        {
            interactor.GetComponent<PlayerStateMachine>().RemainsCanvas.OpenMenu(this);
        }
    }
    public void Disect(GameObject disecter)
    {
        Destroy(this.gameObject);
    }




    private void SetInteractionTrigger()
    {
        interactionTrigger = gameObject.AddComponent<SphereCollider>();
        interactionTrigger.isTrigger = true;
        interactionTrigger.radius = triggerRadius;
        interactionTrigger.center = triggerCenter;
    }
}
