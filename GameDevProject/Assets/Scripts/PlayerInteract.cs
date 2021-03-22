using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    private bool isHoldingItem = false;

    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHoldingItem)
            {
                //get ray out from middle of camera view
                Ray viewRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                //do a raycast with that particular ray, looking for InteractableObjects;
                if (Physics.Raycast(viewRay, out RaycastHit hit, 2.5f, 8))
                {
                    Interactable item = hit.collider.gameObject.GetComponent<Interactable>();
                    item.Interact();
                } 
            }
        }
    }


}
