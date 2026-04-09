using System.Runtime.CompilerServices;
using Unity.Hierarchy;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class PreyBase : AnimalNavBase
{
    [SerializeField] protected float fleeDistance = 12f;

    protected Transform currentPredator;

    protected override void AnimalUpdate()
    {
        if (currentState == AnimalState.Fleeing)
        {
            HandleFlee();
            return;
        }

        base.AnimalUpdate();
    }

    protected virtual void HandleFlee()
    {
        if (currentPredator == null)
        {
            SetState(AnimalState.Roaming);
            return;
        }

        Vector3 fleeDir = (transform.position - currentPredator.position).normalized;
        Vector3 fleePos = transform.position + fleeDir * fleeDistance;

        agent.SetDestination(fleePos);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        PredatorBase predator = other.GetComponent<PredatorBase>();
        if (predator != null)
        {


            if (predator.animalSize >= size)
            {
                currentPredator = other.transform;
                SetState(AnimalState.Fleeing);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == currentPredator)
        {
            currentPredator = null;
            SetState(AnimalState.Roaming);
        }
    }
}
