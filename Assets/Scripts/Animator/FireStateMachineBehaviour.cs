using UnityEngine;

public class FireStateMachineBehaviour : AnimatorStateMachineBehaviour
{
    private int _animHash = 0;

    private void Awake()
    {
        _animHash = Animator.StringToHash(characterState.ToString());
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        //animator.SetTrigger(_animHash);
    }
}
