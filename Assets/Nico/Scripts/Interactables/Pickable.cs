using TMPro;
using UnityEngine;

public class Pickable : MonoBehaviour, IInteractable
{
    [Header("Scriptable Object")]
    [SerializeField] private Item_SO data;

    [Header("Item UI")]
    [SerializeField] private GameObject itemCanvas;
    [SerializeField] private TMP_Text itemName;

    private Item item;
    private Camera mainCamera;



    private void Awake()
    {
        mainCamera = Camera.main;

        Instantiate(data.model, transform);
        name = data.itemName;
        itemName.text = name;

        item = new(data);
    }
    private void LateUpdate()
    {
        itemCanvas.transform.LookAt(transform.position + mainCamera.transform.forward);
    }




    #region Collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            itemCanvas.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            itemCanvas.SetActive(false);
        }
    }
    #endregion




    #region Interact
    public void Interact(GameObject interactor)
    {
        Debug.Log($"Interacted {interactor.name}");
        interactor.GetComponent<Player>().GetInventory().AddItem(item); 
        Destroy(this.gameObject);
    }
    #endregion
}
