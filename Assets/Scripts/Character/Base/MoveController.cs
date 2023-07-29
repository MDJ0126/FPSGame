using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Model.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        private Character _character;
        private float _moveSpeed = 5f;
        private float _rotationSpeed = 15f;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }
        private void LateUpdate()
        {
            Move();
            Rotation();
        }

        private void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
            if (movement != Vector3.zero)
            {
                movement = Camera.main.transform.TransformDirection(movement);
                movement.y = 0f;
                movement.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                _character.Rigidbody.MovePosition(_character.Rigidbody.position + movement * _moveSpeed * Time.deltaTime);
                _character.SetState(eCharacterState.Walk);
            }
            else
            {
                _character.SetState(eCharacterState.Idle);
            }
        }

        private void Rotation()
        {
            float mouseX = Input.GetAxis("Mouse X");
            _character.MyTransform.Rotate(0f, mouseX, 0f);
        }
    }
}