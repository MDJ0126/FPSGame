using UnityEngine;

public abstract class AnimatorStateMachineBehaviour : StateMachineBehaviour
{
    private int _cycle = -1;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _cycle = -1;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (_cycle != (int)stateInfo.normalizedTime)
            OnUpdateCycle(animator, ++_cycle);
    }

    /// <summary>
    /// 반복 스테이트인 경우에는 반복이 될 때마다 반환
    /// </summary>
    /// <param name="cycle"></param>
    public virtual void OnUpdateCycle(Animator animator, int currentCycle)
    {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
