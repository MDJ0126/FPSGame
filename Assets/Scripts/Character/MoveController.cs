using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        private Rigidbody _rigidbody = null;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            _rigidbody.velocity = Vector3.right * 100f * Time.deltaTime;
        }

        public void Move()
        {

        }
    }
}