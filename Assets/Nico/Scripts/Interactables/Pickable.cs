using System.Collections;
using UnityEngine;


public class Pickable : MonoBehaviour, IInteractable
{
    [Header("Scriptable Object")]
    [SerializeField] private Item_SO data;
    public Item_SO Data => data;

    private Item item;



    private void Awake()
    {
        Instantiate(data.model, transform);
        name = data.itemName;

        item = new(data);
    }
    private void Start()
    {
        StartCoroutine(RotateAndFloat());
    }






    #region Interact
    public void Interact(GameObject interactor)
    {
        interactor.GetComponent<Player>().GetInventory().AddItem(item); 
        Destroy(this.gameObject);
    }
    #endregion












    private IEnumerator RotateAndFloat()
    {
        Vector3 startPos = transform.position;

        while (enabled)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * 2f) * 0.3f;
            transform.position = new Vector3(startPos.x, newY, startPos.z);

            transform.Rotate(Vector3.up, 100f * Time.deltaTime, Space.World);

            yield return null;
        }
    }
}
