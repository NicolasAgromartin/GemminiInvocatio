using UnityEngine;




public class Gate : MonoBehaviour, IInteractable
{
    [SerializeField] private int id = 0;



    public void Interact(GameObject interactor)
    {
        //Debug.Log($"Gate! {interactor.name}");

        if (id == 0) Debug.Log("This door desnt need a key");
        else
        {
            if (interactor.GetComponent<Player>().FindKey(id))
            {
                Debug.Log("Can be opened");
                // ejecuta un evento?
                // avisa directamente al scene loader?
                // usa directamente funciones del sceneLoader?
            }
            else
            {
                Debug.Log("Key needed");
            }
        }
    }



}
