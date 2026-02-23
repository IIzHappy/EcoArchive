using UnityEngine;
using UnityEngine.AI;

public class RatNav : PreyBase
{
    [SerializeField] private string boneTag = "Bones"; 
    [SerializeField] private float eatIdleMin = 5f;    
    [SerializeField] private float eatIdleMax = 10f;    

    private Transform boneTarget;
    private float boneIdleTimer;

    protected override void AnimalUpdate()
    {
        
        if (boneTarget != null)
        {
            agent.SetDestination(boneTarget.position);

            float distance = Vector3.Distance(transform.position, boneTarget.position);
            if (distance <= agent.stoppingDistance + 0.5f)
            {
                
                SetState(AnimalState.Resting);
                boneIdleTimer = Random.Range(eatIdleMin, eatIdleMax);
                boneTarget = null;
                agent.ResetPath();
            }
            return; 
        }

      
        if (currentState == AnimalState.Resting)
        {
            HandleEatingRest();
            return;
        }

        
        if (currentPredator != null && Vector3.Distance(transform.position, currentPredator.position) <= fleeDistance)
        {
            SetState(AnimalState.Fleeing);
            HandleFlee();
            return;
        }

        
        base.AnimalUpdate();
    }

    private void HandleEatingRest()
    {
        boneIdleTimer -= Time.deltaTime;
        if (boneIdleTimer <= 0f)
        {
            SetState(AnimalState.Roaming);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
        if (currentState == AnimalState.Roaming && boneTarget == null)
        {
            if (other.CompareTag(boneTag))
            {
                boneTarget = other.transform;
            }
        }

        
        base.OnTriggerEnter(other);
    }
}
