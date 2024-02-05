using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Character
{
    public enum eCharacterState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Attack,
        Dead
    }

    [RequireComponent(typeof(Animator))]
    public class CharacterAnimatorController : MonoBehaviour
    {
        public Animator BoneAnimator { get; private set; } = null;

        private void Awake()
        {
            this.BoneAnimator = GetComponent<Animator>();
        }

        public void SetState(eCharacterState state, Action onFinished = null)
        {
            switch (state)
            {
                case eCharacterState.Idle: SetTrigger(AnimatorHash.Idle); break;
                case eCharacterState.Walk: SetTrigger(AnimatorHash.Walk); break;
                case eCharacterState.Run: SetTrigger(AnimatorHash.Run); break;
                case eCharacterState.Jump: SetTrigger(AnimatorHash.Jump); break;
                case eCharacterState.Attack: SetTrigger(AnimatorHash.Attack); break;
                case eCharacterState.Dead: SetTrigger(AnimatorHash.Dead); break;
            }
        }

        private void SetTrigger(int triggerHash)
        {
            BoneAnimator.SetTrigger(triggerHash);
        }

        private void SetTrigger(string triggerName)
        {
            BoneAnimator.SetTrigger(triggerName);
        }

        private void SetBool(string boolName, bool value)
        {
            BoneAnimator.SetBool(boolName, value);
        }

        private void SetFloat(string floatName, float value)
        {
            BoneAnimator.SetFloat(floatName, value);
        }

        private void SetInteger(string intName, int value)
        {
            BoneAnimator.SetInteger(intName, value);
        }
    }
}