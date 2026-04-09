using UnityEngine;
using UnityEngine.AI;

public class DoeNav : PreyBase
{
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float sprintTime = 2f;
    private float sprintTimer;
    public override string AnimalID => "Doe";

    protected override void HandleFlee()
    {
        if (currentPredator == null)
        {
            SetState(AnimalState.Roaming);
            return;
        }

       
        if (sprintTimer <= 0f)
        {
            agent.speed *= sprintMultiplier;
            sprintTimer = sprintTime;
        }

        sprintTimer -= Time.deltaTime;

        Vector3 fleeDir = (transform.position - currentPredator.position).normalized;
        Vector3 fleePos = transform.position + fleeDir * fleeDistance;

        agent.SetDestination(fleePos);
    }
}
