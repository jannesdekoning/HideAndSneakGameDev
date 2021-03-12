using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsMovement : MonoBehaviour
{
    public float randomAngleLimit = 90;
    public float rotationSpeed = 30;

    Rigidbody rb;
    Quaternion targetDirection, sourceDirection, currentDirection;
    IEnumerator currentSmoothRotation;

    // Start is called before the first frame update
    void Start()
    {
        currentDirection = transform.rotation;
        currentSmoothRotation = SmoothRotation(Random.Range(0.1f, 1.5f));
        StartCoroutine(currentSmoothRotation);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, direction, 0), 1) * Vector3.forward * speed * Time.deltaTime);
        //Debug.DrawLine(transform.position, transform.position + Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,direction,0), 1) * Vector3.forward * 5, Color.magenta);

        //currentDirection = Quaternion.RotateTowards(sourceDirection, targetDirection, rotationSpeed * Time.deltaTime);

        if (currentSmoothRotation == null)
        {
            currentSmoothRotation = SmoothRotation(Random.Range(0.1f, 1.5f));
            StartCoroutine(currentSmoothRotation);
        }

        Debug.DrawLine(transform.position, transform.position + (currentDirection * Vector3.forward), Color.green);
        rb.AddForce(currentDirection * Vector3.forward);
    }

    IEnumerator SmoothRotation(float delay)
    {
        float t = 0f;
        targetDirection.eulerAngles.Set(0, Random.Range(-randomAngleLimit, randomAngleLimit), 0);
        //currentDirection = 
        while (sourceDirection != targetDirection)
        {
            currentDirection = Quaternion.Slerp(sourceDirection, targetDirection, t);
            print($"currentDirection: {currentDirection}");
            print($"sourceDirection: {sourceDirection}");
            print($"targetDirection: {targetDirection}");
            t += 0.2f;
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        StopCoroutine("currentSmoothRotation");
    }

    /*
    IEnumerator ChangeDirection()
    {
        sourceDirection = targetDirection;
        targetDirection.eulerAngles.Set(0, Random.Range(-randomAngleLimit, randomAngleLimit), 0);
        Invoke("ChangeDirection", Random.Range(0.1f, 0.5f));
        Debug.Log($"Source: {sourceDirection}\n Target: {targetDirection}");
    }
    */
}
