using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        private Character _owner = null;
        private Rigidbody _rigidbody = null;
        private NavMeshAgent _agent = null;
        private bool _isGrounded = false;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            if (_agent)
            {
                _agent.updateRotation = false;
            }
        }

        private void LateUpdate()
        {
            UpdateNavMeshSpeed();
            UpdateGroundState();

            // 바닥에 닿아 있는지 여부 업데이트
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

            // 네비 매시 에이전트 속도 실시간 업데이트
            void UpdateNavMeshSpeed()
            {
                if (_agent)
                {
                    _agent.speed = _owner.characterData.moveSpeed * Time.deltaTime;
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
            direction = Vector3.Normalize(direction);
            direction.y = 0;
            if (_isGrounded)
            {
                // 애니메이션 작동
                if (direction != Vector3.zero)
                {
                    _owner.AnimatorController.Walk(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // 블랜드 애니메이션 방향을 위해 따로 입력 받음
                    Vector3 moveVector = direction * _owner.characterData.moveSpeed * Time.deltaTime;

                    // 이동 처리하기
                    if (_agent)
                    {
                        // 네비게이션 메시를 이용하는 경우
                        _agent.velocity = moveVector;
                    }
                    else
                    {
                        // 리지드 바디를 이용하는 경우
                        _rigidbody.velocity = moveVector;
                    }
                }
                else
                {
                    _owner.AnimatorController.Idle();
                }
            }
        }

        /// <summary>
        /// 네비매시를 통해 목적지 이동
        /// </summary>
        /// <param name="dest"></param>
        public bool MoveTo(Vector3 dest)
        {
            if (_agent) return _agent.SetDestination(dest);
            return false;
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