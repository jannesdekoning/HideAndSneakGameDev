using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public Camera cam;

    private GameObject heldItem = null;
    public GameObject HeldItem
    {
        get { return heldItem; }
        set 
        {
            heldItem = value; 
        }
    }



    void Start()
    {
        
    }
    void Update()
    {
        #region Movement
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        #endregion
        #region Interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (HeldItem == null)
            {
                print("attempting to pick up item...");
                //get ray out from middle of camera view
                Ray viewRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                Debug.DrawRay(viewRay.origin, viewRay.direction*2, Color.cyan, 0.5f);

                //do a raycast with that particular ray, looking for InteractableObjects;
                if (Physics.Raycast(viewRay, out RaycastHit hit, 2.5f))
                {
                    print(hit.collider.gameObject.TryGetComponent(out Interactable test));
                    if (hit.collider.gameObject.TryGetComponent(out Interactable obj))
                    {
                        print("interacting...");
                        obj.Interact();
                    }
                }
            }
            else
            {
                HeldItem.GetComponent<ObjectPickUp>().LetGo();
            }
        }
        #endregion
    }
}
