using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SteeringBehaviour : MonoBehaviour
{
    public float wanderRadius = 1f;
    public Vector2 wanderWaitTime = new Vector2(1f, 5f);
    public Material Eyes;

    public LayerMask targetMask;
    public float viewRadius = 5f;
    public float viewAngle = 60f;

    NavMeshAgent agent;
    Coroutine currentWander;

    public Dictionary<GameObject, float> objectsInView = new Dictionary<GameObject, float>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWander = StartCoroutine(Wander());
        StartCoroutine(GetObjectsInView(0.1f));
    }
    void Update()
    {

    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) isTouching = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) isTouching = false;
    }
    */
    //Methods
    static Vector3 GetRandomPosInNav(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;

        NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, radius, -1);

        return navHit.position;
    }
    static Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    void FindObjectsInView()
    {
        Collider[] targetsNearby = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider target in targetsNearby)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0;
            //print($"Angle: {Mathf.Abs(Vector3.Angle(transform.forward, direction))}");

            if (Mathf.Abs(Vector3.Angle(transform.forward, direction)) < viewAngle/2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, direction, distanceToTarget))
                {
                    if (!objectsInView.ContainsKey(target.gameObject))
                    {
                        objectsInView.Add(target.gameObject, Time.time);
                        print($"I added {target.gameObject.name} to the dictionary");
                        if (target.gameObject.CompareTag("Player"))
                        {
                            print($"{target.gameObject.name} is in my view");
                            StartCoroutine(Persuit(target.gameObject));
                        }
                    }
                    else
                    {

                        objectsInView[target.gameObject] = Time.time;
                    }
                }
            }
        }
    }

    //Coroutines
    IEnumerator Wander()
    {
        print("wander");
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
        print("Persuit started");
        StopCoroutine(currentWander);
        Eyes.SetColor("_Color", Color.red);
        while(objectsInView.ContainsKey(target))
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            yield return null;
        }
        Eyes.SetColor("_Color", Color.blue);
        print("persuit ended");
        currentWander = StartCoroutine(Wander());
    }
    IEnumerator GetObjectsInView(float delay)
    {
        while (true)
        {
            FindObjectsInView();
            if (objectsInView.Count > 0)
            {
                var ObjectsToRemove = objectsInView.Where(x=>Time.time > x.Value + 2f).ToArray();
                foreach (var item in ObjectsToRemove)
                {
                    objectsInView.Remove(item.Key);
                    print($"removed {item}");
                }
            }
            yield return new WaitForSeconds(delay); 
        }
    }
}
