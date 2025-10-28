using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Rat : MonoBehaviour
{

    private GameObject[] ratWaypoints;
    private NavMeshAgent agent;
    private GameObject NavDestination;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ratWaypoints = GameObject.FindGameObjectsWithTag("RatPoints");
        Debug.Log(ratWaypoints);
        Debug.Log(ratWaypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        if (agent.destination == null)
        {
            newWaypoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance)
        {
            newWaypoint();
        }
    }

    void newWaypoint()
    {
        var index = Random.Range(0, ratWaypoints.Length);
        NavDestination = ratWaypoints[index];
        agent.destination = NavDestination.transform.position;
        Debug.Log(ratWaypoints[index]);
        Debug.Log(agent.destination);
    }
}
