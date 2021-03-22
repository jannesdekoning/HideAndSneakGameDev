using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SteeringBehaviour : MonoBehaviour
{
    public int damage = 1;
    public float wanderRadius = 1f;
    public Vector2 wanderWaitTime = new Vector2(1f, 5f);
    public Material Eyes;
    public LayerMask targetMask;
    public float viewRadius = 5f;
    public float viewAngle = 60f;
    public bool usePath = false;
    public List<Transform> path;

    NavMeshAgent agent;
    Coroutine currentStateRoutine;
    Coroutine previousStateRoutine;
    IEnumerator defaultState;
    bool isTouching;
    float nextAttack = 0;
    Dictionary<GameObject, float> objectsInView = new Dictionary<GameObject, float>();

    //public enum State
    //{
    //    Wandering,
    //    Persuit,
    //    FollowPath
    //}
    //public State state;
    //public State previousState;

    //Unity methods
    void Start()
    {
        if (usePath) defaultState = FollowPath();
        else defaultState = Wander();

        agent = GetComponent<NavMeshAgent>();
        currentStateRoutine = StartCoroutine(defaultState);
        StartCoroutine(GetObjectsInView(0.1f));
        Eyes.SetColor("_Color", Color.blue);
    }
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouching = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouching = false;
        }
    }

    //Methods
    static Vector3 GetRandomPosInNav(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;

        NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, radius, -1);

        return navHit.position;
    }
    //void ChangeState(IEnumerator nextState)
    //{
    //    print("changing state!");
    //    if (currentStateRoutine != null)
    //    {
    //        StopCoroutine(currentStateRoutine);
    //        print($"Stopped {currentStateRoutine}");
    //    }
    //    previousStateRoutine = currentStateRoutine;
    //    currentStateRoutine = StartCoroutine(nextState);
    //}

    //Coroutines
    IEnumerator Wander()
    {
        print("Wander started");
        Eyes.SetColor("_Color", Color.blue);
        while (true)
        {
            agent.SetDestination(GetRandomPosInNav(transform.position, wanderRadius));
            while (agent.remainingDistance > 0.3f)
            {
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(wanderWaitTime.x, wanderWaitTime.y));
            //currentStateRoutine = StartCoroutine(Wander()); 
        }
    }
    IEnumerator Persuit(GameObject target)
    {
        StopCoroutine(currentStateRoutine);
        print("Persuit started");
        //StopCoroutine(currentState);
        Eyes.SetColor("_Color", Color.red);
        while(objectsInView.ContainsKey(target))
        {
            if (!isTouching)
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }
            else
            {
                PlayerStats player = target.GetComponent<PlayerStats>();
                if (Time.time >= nextAttack)
                {
                    nextAttack = Time.time + 0.5f;
                    player.Health -= damage;
                }
                GetComponent<Rigidbody>().AddRelativeForce(Vector3.back*100);
            }
            yield return null;
        }
        currentStateRoutine = StartCoroutine(defaultState);
    }
    IEnumerator FollowPath()
    {
        print("FollowPath");
        while (true)
        {
            foreach (Transform point in path)
            {

                transform.LookAt(point.position);
                do
                {
                    agent.SetDestination(point.position);
                    yield return null;
                } while (agent.remainingDistance > 0.1f);
                yield return new WaitForSeconds(1);
            }
        }
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
    void FindObjectsInView()
    {
        Collider[] targetsNearby = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider target in targetsNearby)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0;

            if (Mathf.Abs(Vector3.Angle(transform.forward, direction)) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, direction, distanceToTarget))
                {
                    if (!objectsInView.ContainsKey(target.gameObject))
                    {
                        objectsInView.Add(target.gameObject, Time.time);
                        //print($"I added {target.gameObject.name} to the dictionary");
                        if (target.gameObject.CompareTag("Player"))
                        {
                            //print($"{target.gameObject.name} is in my view");
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
}
