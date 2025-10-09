using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorAction : MonoBehaviour
{
    //[SerializeField] private Transform camera;
    //[SerializeField] private float maxDistance = 10f;
    //[SerializeField] private LayerMask useLayers;

    //private void OnEnable()
    //{
      
    //    InputManager.OnInteractAction += HandleInteract;
    //}

    //private void OnDisable()
    //{
        
    //    InputManager.OnInteractAction -= HandleInteract;
    //}

    //private void HandleInteract()
    //{
        

    //    if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxDistance, useLayers))
    //    {
            
           
    //        if (hit.collider.TryGetComponent<Door>(out Door door))
    //        {
    //            if (door.isOpen)
    //            {
                    
    //                door.Close();
    //            }
    //            else
    //            {
                    
    //                door.Open(transform.position);
    //            }
    //        }
    //    }
    //}


    //private void Update()
    //{
    //    Debug.DrawRay(camera.position, camera.forward *30f);
    //}
}

    /* private void Update()
     {
         if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxDistance, useLayers) && (hit.collider.TryGetComponent<Door>(out Door door)))
         {

             if (door.isOpen) 
             {
                 useText.SetText("Abrir con E");
             }
             else 
             {
                 useText.SetText("Cerrar con E");            
             }
             useText.gameObject.SetActive(true);
             useText.transform.position= hit.point - (hit.point- camera.position).normalized *0.01f;
             useText.transform.rotation = Quaternion.LookRotation((hit.point - camera.position).normalized);
         }
         else
         {
             useText.gameObject.SetActive(false);
         }
     }
    */

