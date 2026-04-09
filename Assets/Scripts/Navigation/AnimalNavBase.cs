using UnityEngine;
using UnityEngine.AI;

public class AnimalNavBase : MonoBehaviour
{
    public enum AnimalState
    {
        Idle,
        Roaming,
        Chasing,
        Fleeing,
        Unique,
        Resting
    }

    
    [SerializeField] protected AnimalState currentState;
    public AnimalState CurrentState => currentState; 

 
    [SerializeField] protected float sampleRadius = 10f;
    [SerializeField] protected int maxSampleAttempts = 10;

    [SerializeField] private float idleMinTime = 2f;
    [SerializeField] private float idleMaxTime = 5f;

    [SerializeField] private float playerFleeDistanceCheck = 15f;
    [SerializeField] private float playerFleeDistance = 10f;
    private bool playerFleeActive = false;
    private Transform playerNoise;
    private Vector3 currentFleeTarget;

    private float idleTimer;

    protected NavMeshAgent agent;

    private float moveTimer;
    protected float sampleInterval;

    public virtual string AnimalID => "AnimalID";
    [SerializeField] protected float size = 1f;
    public float animalSize => size;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetState(AnimalState.Roaming);
        SetNewSampleInterval();
    }

    protected virtual void Update()
    {
        moveTimer += Time.deltaTime;

        if (playerFleeActive)
        {
            HandlePlayerFlee();
            return;
        }
        AnimalUpdate();
    }

    protected virtual void AnimalUpdate()
    {
        switch (currentState)
        {
            case AnimalState.Roaming:
                HandleRoaming();
                break;

            case AnimalState.Resting:
                HandleResting();
                break;

            case AnimalState.Chasing:
            case AnimalState.Fleeing:

                break;
        }
    }

    protected virtual void HandleRoaming()
    {
        if (moveTimer >= sampleInterval &&
       !agent.pathPending &&
       agent.remainingDistance <= agent.stoppingDistance)
        {
           
            if (Random.value < 0.2f) 
            {
                SetState(AnimalState.Resting);
                idleTimer = Random.Range(idleMinTime, idleMaxTime);
                return;
            }

            Move();
            moveTimer = 0f;
            SetNewSampleInterval();
        }
    }

    protected virtual void Move()
    {
        for (int i = 0; i < maxSampleAttempts; i++)
        {
            float angle = Random.Range(-135f, 125f);
            Vector3 randDirection =
                Quaternion.Euler(0f, angle, 0f) * transform.forward;

            float randDistance = Random.Range(0f, sampleRadius);
            Vector3 targetPos = transform.position + randDirection * randDistance;

            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit,
                sampleRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return;
            }
        }
    }

    protected virtual void HandleResting()
    {
        idleTimer -= Time.deltaTime;

        if (agent.hasPath)
        {
            agent.ResetPath();
        }

        if (idleTimer <= 0f)
        {
            SetState(AnimalState.Roaming);
        }
    }

    protected virtual void SetState(AnimalState newState)
    {
        currentState = newState;
    }

    protected void SetNewSampleInterval()
    {
        sampleInterval = Random.Range(2f, 6f);
    }

    public void playerFlee(Transform playerNoiseTransform)
    {
        playerNoise = playerNoiseTransform;
        playerFleeActive = true;
        SetState(AnimalState.Fleeing);
        SetPlayerFleeDestination();
    }
    
    private void HandlePlayerFlee()
    {
        if (playerNoise == null)
        {
            StopPlayerFlee();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            float dist = Vector3.Distance(transform.position, playerNoise.position);

            if ( dist < playerFleeDistanceCheck) 
            {
                SetPlayerFleeDestination();
            }
            else
            {
                StopPlayerFlee();
            }
        }

    }

    private void SetPlayerFleeDestination()
    {
        Vector3 awayDirection = (transform.position - playerNoise.position).normalized;
        Vector3 targetPos = transform.position + awayDirection * playerFleeDistance;
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, playerFleeDistance, NavMesh.AllAreas))
        {
            currentFleeTarget = hit.position;
            agent.SetDestination(currentFleeTarget);
        }
    }

    private void StopPlayerFlee()
    {
        playerFleeActive = false;
        playerNoise = null;
        SetState(AnimalState.Roaming);
        moveTimer = 0f;
        SetNewSampleInterval();
    }



}



