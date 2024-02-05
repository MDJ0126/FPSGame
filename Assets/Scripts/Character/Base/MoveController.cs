using System;
using UnityEngine;

namespace Model.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        #region Inspector

        public float moveSpeed = 5f;
        public float rotateSpeed = 200f;
        public float jumpHeight = 6f;

        #endregion

        private Character _character;
        private Rigidbody Rigidbody => _character.Rigidbody;
        private Animator Animator => _character.AnimatorController.BoneAnimator;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            UpdateMove();
            UpdateRotate();
        }

        private void Jump()
        {
            if (CheckGround())
            {
                Vector3 jumpPower = Vector3.up * jumpHeight;
                Rigidbody.AddForce(jumpPower, ForceMode.VelocityChange);
            }

            bool CheckGround()
            {
                if (Physics.Raycast(_character.MyTransform.position, Vector3.down, out RaycastHit hit, 0.2f))
                {
                    return true;
                }
                return false;
            }
        }

        private void UpdateMove()
        {
            Vector3 input = Vector3.zero;
            input.x = Input.GetAxis("Horizontal");
            input.z = Input.GetAxis("Vertical");
            if (input != Vector3.zero)
            {
                Rigidbody.MovePosition(_character.MyTransform.position + _character.MyTransform.rotation.normalized * input * Time.deltaTime * moveSpeed);
                Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            }
        }

        private void UpdateRotate()
        {
            Vector3 dir = Vector3.zero;
            dir.y = Input.GetAxis("Mouse X");
            if (dir != Vector3.zero)
            {
                dir = dir.normalized;
                _character.MyTransform.eulerAngles += dir * rotateSpeed * Time.deltaTime;
            }
        }
    }
}