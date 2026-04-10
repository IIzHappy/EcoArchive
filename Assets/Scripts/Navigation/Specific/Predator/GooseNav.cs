using UnityEngine;

public class GooseNav : PredatorBase
{
    public override string AnimalID => "Goose";

    protected override void OnTriggerEnter(Collider other)
    {
        if (cooldownTimer > 0f) return;

        if (currentState != AnimalState.Roaming) return;

        AnimalNavBase prey = other.GetComponent<AnimalNavBase>();
        if (prey != null)
        {
            if (Random.value < chaseChance && prey.animalSize <= size)
            {
                currentPrey = prey.transform;
                chaseTimer = chaseDuration;
                SetState(AnimalState.Chasing);
            }
        }
    }
}
