using UnityEngine;


 


public class Remains : MonoBehaviour, IInteractable
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


    private void SetInteractionTrigger()
    {
        interactionTrigger = gameObject.AddComponent<SphereCollider>();
        interactionTrigger.isTrigger = true;
        interactionTrigger.radius = triggerRadius;
        interactionTrigger.center = triggerCenter;
    }
    public void SetRemainsData(FiendSO data)
    {
        Data = data;
        //Debug.Log(data.name + " " + data.fiendName);
    }

    public void Interact(GameObject interactor)
    {
        if (interactor.CompareTag("Player"))
        {
            interactor.GetComponent<Necromancy>().SetRemains(this);
        }
    }
}
