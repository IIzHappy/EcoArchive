using UnityEngine;
using UnityEngine.AI;

public class PredatorBase : AnimalNavBase
{
    
    [SerializeField] protected float chaseDuration = 5f;
    [SerializeField] protected float chaseCooldown = 8f;
    [SerializeField] protected float chaseChance = 0.4f;

    protected Transform currentPrey;
    protected float chaseTimer;
    protected float cooldownTimer;

    protected override void Update()
    {
        base.Update();

      
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    protected override void AnimalUpdate()
    {
        if (currentState == AnimalState.Chasing)
        {
            HandleChase();
            return;
        }

        base.AnimalUpdate();
    }

    protected virtual void HandleChase()
    {
        if (currentPrey == null)
        {
            StopChase();
            return;
        }

        chaseTimer -= Time.deltaTime;
        agent.SetDestination(currentPrey.position);

        if (chaseTimer <= 0f)
        {
            StopChase();
        }
    }

    private void StopChase()
    {
        currentPrey = null;
        cooldownTimer = chaseCooldown; 
        SetState(AnimalState.Roaming);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (cooldownTimer > 0f) return;  

        if (currentState != AnimalState.Roaming) return;

        PreyBase prey = other.GetComponent<PreyBase>();
        if (prey != null)
        {
            if (Random.value < chaseChance && prey.animalSize <=size)
            {
                currentPrey = prey.transform;
                chaseTimer = chaseDuration;
                SetState(AnimalState.Chasing);
            }
        }
    }
}
