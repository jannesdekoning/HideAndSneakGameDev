using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5;

    public static bool enableGrow = false;
    public static bool enableShrink = false;
    public static bool isTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))*speed;

        if (enableGrow)
        {
            gameObject.transform.localScale += Vector3.one;
        }
        else if (enableShrink)
        {
            gameObject.transform.localScale -= Vector3.one;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.gameObject.name} has entered the collision");
        if (collision.gameObject.name == "Boid")
        {
            isTouching = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log($"{collision.gameObject.name} has exited the collision");
        if (collision.gameObject.name == "Boid")
        {
            isTouching = false;
        }
    }
}
