using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class AnimatorController : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>(true);
        }

        public void Idle()
        {
            _animator.SetBool(AnimHash.IsWalk, false);
        }

        public void Walk(float velocityX, float velocityZ)
        {
            _animator.SetBool(AnimHash.IsWalk, true);
            _animator.SetFloat(AnimHash.VelocityX, velocityX);
            _animator.SetFloat(AnimHash.VelocityZ, velocityZ);
        }
    }
}