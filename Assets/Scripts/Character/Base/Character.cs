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

        private void Awake()
        {
            Rigidbody = this.GetComponent<Rigidbody>();

            // AddComponents
            this.gameObject.AddComponent<MoveController>();
        }
    }
}