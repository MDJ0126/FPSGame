using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class AnimatorController : MonoBehaviour
    {
        private Animator _animator;
        private Rig _rig;
        public TwoBoneIKConstraint LeftHand { get; private set; } = null;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>(true);
            FindHands();
        }

        public void Idle()
        {
            _animator.SetBool(AnimHash.IsWalk, false);
        }

        public void Walk(float velocityX, float velocityZ)
        {
            _animator.SetBool(AnimHash.IsWalk, true);
            _animator.SetFloat(AnimHash.VelocityX, Mathf.Lerp(_animator.GetFloat(AnimHash.VelocityX), velocityX, 0.1f));
            _animator.SetFloat(AnimHash.VelocityZ, Mathf.Lerp(_animator.GetFloat(AnimHash.VelocityZ), velocityZ, 0.1f));
        }

        private void FindHands()
        {
            var childs = this.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < childs.Length; i++)
            {
                Transform child = childs[i];
                if (child.name.Equals("LeftHand"))
                {
                    LeftHand = child.GetComponent<TwoBoneIKConstraint>();
                }
            }
            _rig = this.GetComponentInChildren<Rig>();
        }
    }
}