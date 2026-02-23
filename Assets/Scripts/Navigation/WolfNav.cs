using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class WolfNav : PredatorBase
{
    [SerializeField] private float packRadius = 15f;       
    [SerializeField] private float packWeight = 0.3f;      
    [SerializeField] private float formationRadius = 2f;   

   
    [SerializeField] private float chaseCooldown = 5f;     
    [SerializeField] private float chaseDuration = 8f;     
    [SerializeField] private float chaseChance = 0.5f;     

  
    [SerializeField] private float idleMin = 2f;
    [SerializeField] private float idleMax = 4f;

    private Transform targetPrey;
    private float chaseTimer;
    private float cooldownTimer;
    private bool isIdle = false;

    protected override void Update()
    {
        base.Update();

       
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (chaseTimer > 0f)
            chaseTimer -= Time.deltaTime;

        
        if (currentState == AnimalState.Resting && isIdle)
        {
            HandleIdleRest();
            return;
        }

      
        if (currentState == AnimalState.Chasing)
        {
            HandleChase();
            return;
        }

       
        base.AnimalUpdate();
    }

    protected override void AnimalUpdate()
    {
     
    }

    protected override void HandleChase()
    {
        if (targetPrey == null || chaseTimer <= 0f)
        {
            StopChase();
            return;
        }

       
        Vector3 targetPos = targetPrey.position;

       
        Collider[] nearby = Physics.OverlapSphere(transform.position, packRadius);
        Vector3 packCenter = Vector3.zero;
        int wolfCount = 0;

        foreach (Collider col in nearby)
        {
            WolfNav otherWolf = col.GetComponent<WolfNav>();
            if (otherWolf != null && otherWolf != this && otherWolf.CurrentState == AnimalState.Chasing)
            {
                packCenter += otherWolf.transform.position;
                wolfCount++;
            }
        }

        if (wolfCount > 0)
        {
            packCenter /= wolfCount;
            targetPos = Vector3.Lerp(targetPos, packCenter, packWeight);
        }

      
        Vector3 offset = Random.insideUnitSphere * formationRadius;
        offset.y = 0f;
        targetPos += offset;

        
        agent.SetDestination(targetPos);
    }

    private void StopChase()
    {
        targetPrey = null;
        cooldownTimer = chaseCooldown;
        chaseTimer = 0f;
        SetState(AnimalState.Roaming);
        StartIdle();
    }

    private void StartIdle()
    {
        isIdle = true;
        SetState(AnimalState.Resting);
        agent.ResetPath();
        chaseTimer = 0f;
    }

    private void HandleIdleRest()
    {
        if (idleMax <= 0f) return;

        if (isIdle)
        {
            float idleTime = Random.Range(idleMin, idleMax);
            idleMax -= Time.deltaTime;

            if (idleMax <= 0f)
            {
                isIdle = false;
                SetState(AnimalState.Roaming);
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
        WolfNav otherWolf = other.GetComponent<WolfNav>();
        if (otherWolf != null && otherWolf.CurrentState == AnimalState.Chasing && currentState == AnimalState.Roaming)
        {
            targetPrey = otherWolf.targetPrey;
            chaseTimer = chaseDuration;
            SetState(AnimalState.Chasing);
            return;
        }

       
        PreyBase prey = other.GetComponent<PreyBase>();
        if (prey != null && currentState == AnimalState.Roaming && cooldownTimer <= 0f)
        {
            if (Random.value < chaseChance)
            {
                targetPrey = prey.transform;
                chaseTimer = chaseDuration;
                SetState(AnimalState.Chasing);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == targetPrey)
        {
            StopChase();
        }
    }
}
