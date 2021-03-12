using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerable Wander()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
    }
}
