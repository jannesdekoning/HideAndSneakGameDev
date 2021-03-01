using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsMovement : MonoBehaviour
{
    public float randomAngleLimit = 20;
    public float rotationSpeed = 30;

    private Rigidbody rb;
    private Quaternion targetDirection, sourceDirection, currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        currentDirection = transform.rotation;
        rb = GetComponent<Rigidbody>();
        Invoke("ChangeDirection", Random.Range(0.1f, 0.5f));

        StartCoroutine(ChangeDirection)
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, direction, 0), 1) * Vector3.forward * speed * Time.deltaTime);
        //Debug.DrawLine(transform.position, transform.position + Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,direction,0), 1) * Vector3.forward * 5, Color.magenta);

        currentDirection = Quaternion.RotateTowards(sourceDirection, targetDirection, rotationSpeed * Time.deltaTime);

        Debug.DrawLine(transform.position, transform.position + (currentDirection * Vector3.forward), Color.green);
        rb.AddForce(currentDirection * Vector3.forward);
    }

    IEnumerator ChangeDirection()
    {
        sourceDirection = targetDirection;
        targetDirection.eulerAngles.Set(0, Random.Range(-randomAngleLimit, randomAngleLimit), 0);
        Invoke("ChangeDirection", Random.Range(0.1f, 0.5f));
        Debug.Log($"Source: {sourceDirection}\n Target: {targetDirection}");
    }

    IEnumerator SmoothRotation()
    {

        yield return null
    }
}
