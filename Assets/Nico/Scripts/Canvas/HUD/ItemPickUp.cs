using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemPickUp : MonoBehaviour
{
    private TMP_Text itemName;
    private Image background;

    private readonly Queue<ItemType> itemQueue = new Queue<ItemType>();
    private Coroutine queueRoutine;







    private void Awake()
    {
        itemName = GetComponentInChildren<TMP_Text>(true);
        background = GetComponentInChildren<Image>(true);
    }

    private void OnEnable()
    {
        Inventory.OnItemAdded += OnItemAdded;
    }

    private void OnDisable()
    {
        Inventory.OnItemAdded -= OnItemAdded;
    }










    private void OnItemAdded(ItemType itemType)
    {
        itemQueue.Enqueue(itemType);

        // si no hay corrutina procesando la cola, iniciarla
        if (queueRoutine == null)
            queueRoutine = StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        while (itemQueue.Count > 0)
        {
            ItemType current = itemQueue.Dequeue();
            itemName.text = $"{current} added to the inventory";

            // Ejecuta el flujo de fade completo
            yield return StartCoroutine(DisplayFlow());
        }

        // al terminar, liberar referencia
        queueRoutine = null;
    }

    private IEnumerator DisplayFlow()
    {
        // Aseguramos que los elementos estén visibles antes del fade
        background.gameObject.SetActive(true);
        itemName.gameObject.SetActive(true);

        yield return StartCoroutine(UIEffects.FadeIn(background, 2.5f));
        yield return StartCoroutine(UIEffects.FadeIn(itemName, 2.5f));

        yield return new WaitForSeconds(1.25f);

        StartCoroutine(UIEffects.FadeOut(itemName, 4f));
        yield return StartCoroutine(UIEffects.FadeOut(background, 4f));
    }
}
