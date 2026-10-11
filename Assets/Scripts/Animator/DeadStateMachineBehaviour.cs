using UnityEngine;

public class DeadStateMachineBehaviour : AnimatorStateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (stateInfo.normalizedTime > 1f)
        {
            OnFinishedEvent();
        }
    }
}
