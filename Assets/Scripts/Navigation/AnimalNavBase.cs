using System.Runtime.CompilerServices;
using Unity.Hierarchy;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class AnimalNavBase : MonoBehaviour
{
    [SerializeField] float sampleRadius = 10f;
    [SerializeField] float sampleInterval;
    [SerializeField] int maxSampleAttempts = 10;
    private NavMeshAgent agent;
    private float moveTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sampleInterval = Random.Range(2f, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= sampleInterval && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Move();
            moveTimer = 0f;
            sampleInterval = Random.Range(2f, 6f);
        }
    }

    private void Move()
    {
        for (int i = 0; i < maxSampleAttempts; i++)
        {
            float angle = Random.Range(-135f, 125f);
            Vector3 randDirection = Quaternion.Euler(0f, angle, 0f) * transform.forward;
            float randDistance = Random.Range(0f, sampleRadius);

            Vector3 targetPOS = transform.position + randDirection * randDistance;
            //randDirection.y = 0f;

            if (NavMesh.SamplePosition(targetPOS, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return;
            }
        }
    }
}
