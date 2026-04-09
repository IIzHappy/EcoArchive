using UnityEngine;
using UnityEngine.AI;

public class unMeshed : AnimalNavBase
{
    protected override void Awake()
    {
        SetState(AnimalState.Roaming);
    }

    protected override void Update ()
    {

    }

   
}
