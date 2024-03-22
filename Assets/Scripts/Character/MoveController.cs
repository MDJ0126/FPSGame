using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        private Character _owner = null;
        private Rigidbody _rigidbody = null;
        private float _tempSpeed = 1000f;
        private Vector3 _velocity = Vector3.zero;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            if (_velocity != Vector3.zero)
            {
                _rigidbody.velocity = _velocity;
                _velocity = Vector3.zero;
            }
        }

        public void Move(Vector3 direction)
        {
            _velocity = direction * _tempSpeed * Time.deltaTime;
        }
    }
}