using UnityEngine;

public class RandomIdleStateMachineBehaviour : AnimatorStateMachineBehaviour
{
    private const int STACK_MAX = 100;
    private int _stack = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _stack = 0;
        animator.SetBool(AnimHash.SpecialIdle, false);
    }

    public override void OnUpdateCycle(Animator animator, int currentCycle)
    {
        base.OnUpdateCycle(animator, currentCycle);
        UpdateStack(animator);
    }

    private void UpdateStack(Animator animator)
    {
        _stack += Random.Range(0, STACK_MAX);
        if (_stack >= STACK_MAX)
        {
            animator.SetBool(AnimHash.SpecialIdle, true);
        }
    }
}