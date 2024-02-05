using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Character
{
    public abstract class Character : MonoBehaviour
    {
        private Transform _transform;
        public Transform MyTransform
        {
            get
            {
                if (!_transform) _transform = GetComponent<Transform>();
                return _transform;
            }
        }

        public Rigidbody Rigidbody { get; private set; } = null;
        public CharacterAnimatorController AnimatorController { get; private set; } = null;

        private void Awake()
        {
            // Base Components
            this.Rigidbody = this.GetComponent<Rigidbody>();
            this.AnimatorController = this.GetComponentInChildren<CharacterAnimatorController>();

            // Add Components
            this.gameObject.AddComponent<MoveController>();
        }

        public void SetState(eCharacterState state, Action onFinished = null)
        {
            AnimatorController.SetState(state, onFinished);
        }
    }
}