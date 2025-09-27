using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    public void Interact(GameObject interactor)
    {
        Debug.Log($"Gate! {interactor.name}");

        Item key = interactor.GetComponent<Player>().UseKey();

        if(key != null) Destroy(this.gameObject);
    }
}
