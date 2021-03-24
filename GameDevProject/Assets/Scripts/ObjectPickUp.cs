using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : Interactable
{
    public Rigidbody rb;
    PlayerMovement playerControls;
    Camera cam;
    Coroutine updatePosition;

    private void Start()
    {
        playerControls = GameObject.Find("FPS-Player").GetComponent<PlayerMovement>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public override void Interact()
    {
        print("You pick me up!");
        //rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = false;
        transform.parent = cam.transform;
        playerControls.HeldItem = this.gameObject;
        //updatePosition = StartCoroutine(UpdatePosition());
    }

    public void LetGo()
    {
        print("You dropped me!");
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        transform.parent = null;
        playerControls.HeldItem = null;
        //StopCoroutine(updatePosition);
    }

    IEnumerator UpdatePosition()
    {
        //Vector3 dest = cam.transform.forward;
        //if (Vector3.Distance(transform.position, dest) > 0.01f)
        //{

        //}
        Vector3 direction = (transform.position + cam.transform.forward).normalized;
        float distance = Vector3.Distance(transform.position, cam.transform.forward);
        rb.AddRelativeForce(direction*distance, ForceMode.VelocityChange);
        yield return null;
    }
}
