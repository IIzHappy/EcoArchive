using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Deer : MonoBehaviour
{

    private GameObject[] deerWaypoints;
    private NavMeshAgent agent;
    private GameObject NavDestination;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deerWaypoints = GameObject.FindGameObjectsWithTag("DeerPoints");
        Debug.Log(deerWaypoints);
        Debug.Log(deerWaypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        if (agent.destination == null)
        {
            newWaypoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance)
        {
            newWaypoint();
        }
    }

    void newWaypoint()
    {
        var index = Random.Range(0, deerWaypoints.Length);
        NavDestination = deerWaypoints[index];
        agent.destination = NavDestination.transform.position;
        Debug.Log(deerWaypoints[index]);
        Debug.Log(agent.destination);   
    }
}
