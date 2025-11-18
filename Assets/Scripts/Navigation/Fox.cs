using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Fox : MonoBehaviour
{
    public float chaseDistance;
    public Transform foxPosition;
    private GameObject[] foxWaypoints;
    private NavMeshAgent agent;
    private GameObject NavDestination;
    private Quaternion Rotation;
    private GameObject[] prey;
    private GameObject chasePrey;
    [SerializeField] bool isChasing = false;
    [SerializeField] float chaseTime = 10f;
    [SerializeField] float chaseTimer = 0f;
    [SerializeField] float chaseRecharge = 10f;
    public bool chaseRecharging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foxPosition = transform;
        prey = GameObject.FindGameObjectsWithTag("Prey");
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
        if(chaseRecharging == true)
        {
            chaseRecharge -= Time.deltaTime;
            if(chaseRecharge <= 0f)
            {
                chaseRecharge = 10f;
                chaseRecharging = false;
            }
        }
        foreach (GameObject target in prey)
        {
            if (target != null && !isChasing && !chaseRecharging) {
                float distance = Vector3.Distance(foxPosition.position, target.transform.position);
                if (distance < chaseDistance && !isChasing)
                {
                    isChasing = true;
                    chasePrey = target;
                    Debug.Log("isChasing");

                }
            }
        }
        Rotation = Quaternion.Euler(0, +90f, 0);
        if (!isChasing)
        {
            // gameObject.transform.LookAt(agent.destination + Rotation);
            if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance)
            {
                newWaypoint();
            }
        }
        else
        {
            Chase();
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= chaseTime)
            {
                isChasing = false;
                chaseRecharging = true;
                chaseTimer = 0;
                newWaypoint();
                Debug.Log("AbandonChase");
            } 
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

    void Chase()
    {
        agent.SetDestination(chasePrey.transform.position);
    }
}
