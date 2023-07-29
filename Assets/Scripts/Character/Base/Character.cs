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

        [HideInInspector] public Rigidbody Rigidbody;
        private CharacterAnimatorController _animatorController;

        private void Awake()
        {
            // Base Components
            Rigidbody = this.GetComponent<Rigidbody>();
            _animatorController = this.GetComponentInChildren<CharacterAnimatorController>();

            // Add Components
            this.gameObject.AddComponent<MoveController>();
        }

        public void SetState(eCharacterState state, Action onFinished = null)
        {
            _animatorController.SetState(state, onFinished);
        }
    }
}