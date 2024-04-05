using UnityEngine;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        private Character _owner = null;
        private Rigidbody _rigidbody = null;
        private float _tempSpeed = 300f;
        private Vector3 _rotate = Vector3.zero;
        private Vector3 _velocity = Vector3.zero;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody>();
            _rotate = _owner.MyTransform.localRotation.eulerAngles;
        }

        private void LateUpdate()
        {
            if (_velocity != Vector3.zero)
            {
                _owner.AnimatorController.Walk(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                _rigidbody.velocity = _velocity;
                _velocity = Vector3.zero;
            }
            else
            {
                _owner.AnimatorController.Idle();
            }

            if (_rotate != Vector3.zero)
            {
                _owner.MyTransform.eulerAngles += _rotate;
                _rotate = Vector3.zero;
            }
        }

        public void AddRotate(Vector3 rotate)
        {
            _rotate = rotate;
        }

        public void Move(Vector3 direction)
        {
            _velocity = direction * _tempSpeed * Time.deltaTime;
            //_rigidbody.MovePosition(_owner.MyTransform.position + (direction * _tempSpeed * Time.deltaTime));
        }
    }
}