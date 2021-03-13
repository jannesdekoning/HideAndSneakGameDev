using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteeringBehaviour : MonoBehaviour
{
    public float wanderRadius = 1;
    public Vector2 wanderWaitTime = new Vector2(1f, 5f);
    public Material Eyes;

    NavMeshAgent agent;
    Coroutine currentWander;
    public List<GameObject> nearbyObjects;
    bool isTouching = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWander = StartCoroutine(Wander());
    }
    private void Update()
    {

    }

    private void OnTriggerEnter(Collider subject)
    {
        print(subject.gameObject);
        nearbyObjects.Add(subject.gameObject);
        if (subject.gameObject.tag == "Player") StartCoroutine(Persuit(subject.gameObject));
    }
    private void OnTriggerExit(Collider subject)
    {
        nearbyObjects.Remove(subject.gameObject);
        if (subject.gameObject.tag == "Player")
        {
            print("Ive lost track of the player...");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") isTouching = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player") isTouching = false;
    }

    //Methods
    public static Vector3 GetRandomPosInNav(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, radius, -1);

        return navHit.position;
    }
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    //Coroutines
    IEnumerator Wander()
    {
        agent.SetDestination(GetRandomPosInNav(transform.position, wanderRadius));
        while (agent.remainingDistance > 0.1)
        {
            yield return null;
        }
        yield return new WaitForSeconds(Random.Range(wanderWaitTime.x, wanderWaitTime.y));
        currentWander = StartCoroutine(Wander());
    }
    IEnumerator Persuit(GameObject target)
    {
        StopCoroutine(currentWander);
        Eyes.SetColor("_Color", Color.red);
        while(nearbyObjects.Exists(x => x == target))
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 0.4)
            {
                agent.SetDestination(target.transform.position);
                yield return null;
            }
            else
            {

            }
            yield return null;
        }
        Eyes.SetColor("_Color", Color.blue);
        currentWander = StartCoroutine(Wander());
    }
}
