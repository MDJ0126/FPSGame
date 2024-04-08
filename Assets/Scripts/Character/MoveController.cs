using UnityEngine;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        private Character _owner = null;
        private Rigidbody _rigidbody = null;
        private Vector3 _velocity = Vector3.zero;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            _rigidbody.velocity = _velocity;
            if (_velocity != Vector3.zero)
            {
                _owner.AnimatorController.Walk(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                _velocity = Vector3.zero;
            }
            else
            {
                _owner.AnimatorController.Idle();
            }
        }

        /// <summary>
        /// 바라보기
        /// </summary>
        /// <param name="character"></param>
        public void LootAt(Character character)
        {
            LootAt(character.MyTransform.position);
        }

        /// <summary>
        /// 바라보기
        /// </summary>
        /// <param name="pos"></param>
        public void LootAt(Vector3 pos)
        {
            _owner.MyTransform.LookAt(new Vector3(pos.x, _owner.MyTransform.position.y, pos.z));
        }

        /// <summary>
        /// 캐릭터 회전
        /// </summary>
        /// <param name="rotate"></param>
        public void UpdateRotation(Vector3 rotate)
        {
            Vector3 angle = _owner.MyTransform.eulerAngles;
            _owner.MyTransform.rotation = Quaternion.Euler(angle.x - rotate.y, angle.y + rotate.x, angle.z);
        }

        /// <summary>
        /// 이동
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Vector3 direction)
        {
            direction = Vector3.Normalize(direction);
            _velocity = direction * _owner.characterData.moveSpeed * Time.deltaTime;
            _velocity.y = 0f;
            //_rigidbody.MovePosition(_owner.MyTransform.position + (direction * _tempSpeed * Time.deltaTime));
        }
    }
}