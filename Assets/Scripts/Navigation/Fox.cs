using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Fox : MonoBehaviour
{

    private GameObject[] foxWaypoints;
    private NavMeshAgent agent;
    private GameObject NavDestination;
    private Quaternion Rotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foxWaypoints = GameObject.FindGameObjectsWithTag("FoxPoints");
        Debug.Log(foxWaypoints);
        Debug.Log(foxWaypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        if (agent.destination == null)
        {
            newWaypoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotation = Quaternion.Euler(0, +90f, 0);
       // gameObject.transform.LookAt(agent.destination + Rotation);
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance)
        {
            newWaypoint();
        }
    }

    void newWaypoint()
    {
        var index = Random.Range(0, foxWaypoints.Length);
        NavDestination = foxWaypoints[index];
        agent.destination = NavDestination.transform.position;
        Debug.Log(foxWaypoints[index]);
        Debug.Log(agent.destination);
    }
}
