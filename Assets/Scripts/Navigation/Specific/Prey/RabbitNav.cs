using UnityEngine;
using UnityEngine.AI;

public class RabbitNav : PreyBase
{
    protected override void HandleFlee()
    {
        if (currentPredator == null)
        {
            SetState(AnimalState.Roaming);
            return;
        }

       
        Vector3 fleeDir = (transform.position - currentPredator.position).normalized;
        Vector3 randomOffset = Quaternion.Euler(0, Random.Range(-30f, 30f), 0) * fleeDir;

        Vector3 fleePos = transform.position + randomOffset * fleeDistance;
        agent.SetDestination(fleePos);
    }
}
