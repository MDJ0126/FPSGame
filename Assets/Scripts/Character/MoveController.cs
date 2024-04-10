using Unity.VisualScripting;
using UnityEngine;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        private Character _owner = null;
        private Rigidbody _rigidbody = null;
        private Vector3 _velocity = Vector3.zero;
        private bool _isGrounded = false;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            UpdateGroundState();
            void UpdateGroundState()
            {
                LayerMask ignoreLayer = 1 << (int)eLayer.IgnoreRaycast;
                if (Physics.Raycast(_owner.MyTransform.position + (Vector3.up * 0.2f), Vector3.down, out var hit, 0.4f, ~ignoreLayer))
                {
                    _isGrounded = true;
                }
                else
                {
                    _isGrounded = false;
                }
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
            Vector3 moveVector = Vector3.zero;
            if (_isGrounded)
            {
                direction = Vector3.Normalize(direction);
                direction.y = 0;

                if (direction != Vector3.zero)
                {
                    moveVector = direction * _owner.characterData.moveSpeed * Time.deltaTime;
                }
            }

            if (moveVector != Vector3.zero)
            {
                _owner.AnimatorController.Walk(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // 블랜드 애니메이션 방향을 위해 따로 입력 받음
                _rigidbody.velocity = new Vector3(moveVector.x, _rigidbody.velocity.y, moveVector.z);
            }
            else
            {
                _owner.AnimatorController.Idle();
            }
        }

        /// <summary>
        /// 점프
        /// </summary>
        public void Jump()
        {
            if (_isGrounded)
            {
                _rigidbody.velocity += Vector3.up * _owner.characterData.jumpWeight;
            }
        }
    }
}