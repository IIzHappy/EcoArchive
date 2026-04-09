using UnityEngine;
using UnityEngine.AI;

public class FoxNav : PredatorBase
{
    [SerializeField] private string playerTag = "Player";   
    [SerializeField] private float safeDistance = 15f;      
    [SerializeField] private float fleeMultiplier = 1.5f;  

    private Transform playerTransform;
    public override string AnimalID => "Fox";

    protected override void AnimalUpdate()
    {
        
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }

      
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < safeDistance)
        {
            SetState(AnimalState.Fleeing);
            HandlePlayerFlee();
            return;
        }

       
        base.AnimalUpdate();
    }

    private void HandlePlayerFlee()
    {
        if (playerTransform == null)
        {
            SetState(AnimalState.Roaming);
            return;
        }

        
        Vector3 fleeDir = (transform.position - playerTransform.position).normalized;

       
        Vector3 fleePos = transform.position + fleeDir * safeDistance;

        
        agent.speed = agent.speed * fleeMultiplier;
        agent.SetDestination(fleePos);
    }
    
}
