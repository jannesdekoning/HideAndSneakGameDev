using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;

    private GameObject target;
    private Rigidbody rb;
    private Material mat;
    private Vector3 direction;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        mat = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + direction, Color.red);
        direction = target.transform.position - this.transform.position;
        rb.AddForce(direction.normalized * speed/Vector3.Distance(target.transform.position, this.transform.position));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == target)
        {
            mat.color = Color.green;
        }
    }
}
