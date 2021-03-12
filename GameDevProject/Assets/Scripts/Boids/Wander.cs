using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    Rigidbody rb;
    public float rotationLimit;
    public float speed;

    IEnumerator currentSmoothRotation;
    bool isRotating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine("RotateRandom");
    }

    // Update is called once per frame
    void Update()
    {
        print($"Direction: {transform.rotation}");
        Debug.DrawLine(transform.position, transform.position + (transform.rotation * Vector3.forward), Color.magenta);
        rb.MovePosition(transform.position + (transform.rotation * Vector3.forward).normalized * speed);
    }

    IEnumerator SmoothRotation(GameObject source, Quaternion target, float speed, float delay)
    {
        print("SmoothRotation has been called");
        yield return new WaitForSeconds(delay);
        float t = 0;
        //float start = source.transform.rotation;
        while (source.transform.rotation != target)
        {
            source.transform.rotation = Quaternion.Slerp(source.transform.rotation, target, t);
            t += speed;
            yield return null;
            print($"source: {source}");
        }
        isRotating = false;
    }

    IEnumerator RotateRandom()
    {
        print("RotateRandom has started");
        while (true)
        {
            currentSmoothRotation = SmoothRotation(gameObject, Quaternion.Euler(0, Random.Range(-rotationLimit, rotationLimit), 0), 0.01f, Random.Range(0f, 0.5f));
            StartCoroutine(currentSmoothRotation);
            isRotating = true;
            yield return new WaitUntil(() => !isRotating);
        }
    }
}
