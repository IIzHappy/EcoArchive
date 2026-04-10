using UnityEngine;

public class TurtleNav : PreyBase
{
    public override string AnimalID => "Turtle";

    protected override void UpdateAnimator()
    {
        if (animator == null) return;

        bool isIdle = currentState == AnimalState.Resting;
        bool isRoaming = !isIdle;

        animator.SetBool(IsIdle, isIdle);
        animator.SetBool(IsRoaming, isRoaming);
        animator.SetBool(IsFleeing, false);
    }
}
