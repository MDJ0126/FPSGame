using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace FPSGame.Character
{
    public abstract class Character : MonoBehaviour
    {
        public static float CHARACTER_HEIGHT_CENTER = 1f;

        #region Inspector

        public CharacterData characterData;
        public Transform aim;
        public MultiAimConstraint weaponAim;

        [Header("HUD")]
        public Transform hudPos;

        #endregion

        public delegate void OnChangeCharacterStateEvent(eCharacterState state);
        private event OnChangeCharacterStateEvent _onChangeCharacterState = null;
        public event OnChangeCharacterStateEvent OnChangeCharacterState
        {
            add
            {
                _onChangeCharacterState -= value;
                _onChangeCharacterState += value;
            }
            remove
            {
                _onChangeCharacterState -= value;
            }
        }

        public delegate void OnCharacterEvent(Character character);
        private event OnCharacterEvent _onDead = null;
        public event OnCharacterEvent OnDead
        {
            add
            {
                _onDead -= value;
                _onDead += value;
            }
            remove
            {
                _onDead -= value;
            }
        }
        private event OnCharacterEvent _onChangedHitPoint = null;
        public event OnCharacterEvent OnChangedHitPoint
        {
            add
            {
                _onChangedHitPoint -= value;
                _onChangedHitPoint += value;
            }
            remove
            {
                _onChangedHitPoint -= value;
            }
        }
        public delegate void OnSendLogEvent(string message);
        private event OnSendLogEvent _onSendLog = null;
        public event OnSendLogEvent OnSendLog
        {
            add
            {
                _onSendLog -= value;
                _onSendLog += value;
            }
            remove
            {
                _onSendLog -= value;
            }
        }

        /// <summary>
        /// 플레이어 정보
        /// </summary>
        public PlayerInfo PlayerInfo { get; private set; } = null;
        private Transform _myTransform = null;
        public Transform MyTransform
        {
            get
            {
                if (_myTransform == null)
                    _myTransform = transform;
                return _myTransform;
            }
        }
        /// <summary>
        /// 애니메이터 컨트롤러
        /// </summary>
        public AnimatorController AnimatorController { get; private set; } = null;
        /// <summary>
        /// 이동 컨트롤러
        /// </summary>
        public MoveController MoveController { get; private set; } = null;
        /// <summary>
        /// 무기 핸들러
        /// </summary>
        public WeaponHandler WeaponHandler { get; private set; } = null;
        /// <summary>
        /// 타겟 탐지
        /// </summary>
        public DetectTarget DetectTarget { get; private set; } = null;
        /// <summary>
        /// 콜라이더
        /// </summary>
        public CapsuleCollider Collider { get; private set; } = null;
        /// <summary>
        /// 팀 번호
        /// </summary>
        public byte TeamNember { get; protected set; } = 0;
        /// <summary>
        /// 현재 체력
        /// </summary>
        public float Hp { get; private set; } = 100f;
        /// <summary>
        /// 사망 여부
        /// </summary>
        public bool IsDead => Hp <= 0f;
        /// <summary>
        /// 에임 높이
        /// </summary>
        public float AimHeight { get; private set; } = 0f;
        /// <summary>
        /// 에임 범위
        /// </summary>
        public float AimDistance { get; private set; } = 0f;

        protected virtual void Awake()
        {
            if (this.AnimatorController == null)
                this.AnimatorController = this.gameObject.AddComponent<AnimatorController>();
            if (this.MoveController == null)
                this.MoveController = this.gameObject.AddComponent<MoveController>();
            if (this.WeaponHandler == null)
                this.WeaponHandler = this.gameObject.AddComponent<WeaponHandler>();
            if (this.DetectTarget == null)
                this.DetectTarget = DetectTarget.AddComponent(this);
            Collider = this.gameObject.GetComponent<CapsuleCollider>();
            AimHeight = aim.localPosition.y;
            AimDistance = aim.localPosition.z;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public virtual void Initiailize()
        {
            this.Collider.enabled = true;
            this.Hp = characterData.maxHp;
        }

        /// <summary>
        /// 플레이어 정보 세팅
        /// </summary>
        /// <param name="playerInfo"></param>
        public void SetPlayerInfo(PlayerInfo playerInfo)
        {
            this.PlayerInfo = playerInfo;
        }

        /// <summary>
        /// 팀 세팅
        /// </summary>
        /// <param name="teamNember"></param>
        public virtual void SetTeam(byte teamNember)
        {
            this.TeamNember = teamNember;
        }

        /// <summary>
        /// 캐릭터 에임 위치 조정
        /// </summary>
        /// <param name="target"></param>
        public void SetAim(Character target)
        {
            if (target != null)
            {
                Vector3 aimCenter = this.MyTransform.position;
                aimCenter.y = AimHeight;
                Vector3 targetCenter = target.MyTransform.position;
                targetCenter.y = CHARACTER_HEIGHT_CENTER;
                Vector3 direction = targetCenter - aimCenter;
                aim.position = aimCenter + direction * AimDistance;
            }
        }

        /// <summary>
        /// 점수 추가
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(int score)
        {
            if (this.IsDead) return;
            this.PlayerInfo.AddScore(score);
        }

        /// <summary>
        /// 데미지 피격
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(Character attacker, float damage)
        {
            if (this.Hp > 0f)
            {
                this.Hp -= damage;
                _onChangedHitPoint?.Invoke(this);
                if (this.Hp <= 0f) Dead(attacker);
            }
        }

        /// <summary>
        /// 상태 전환
        /// </summary>
        /// <param name="state"></param>
        /// <param name="onFinished"></param>
        public void SetState(eCharacterState state, Action onFinished = null)
        {
            _onChangeCharacterState?.Invoke(state);
            this.AnimatorController.SetState(state, onFinished);
        }

        /// <summary>
        /// 로그 보내기 이벤트
        /// </summary>
        public void SendLog(string message)
        {
            _onSendLog?.Invoke(message);
        }

        /// <summary>
        /// 사망
        /// </summary>
        public virtual void Dead(Character attacker)
        {
            if (!(this is PlayerCharacter)) Collider.enabled = false;
            SetState(eCharacterState.Dead, () =>
            {
                OnDeadState();
            });
            _onDead?.Invoke(this);
        }

        /// <summary>
        /// 사망 애니메이션이 끝나는 경우 호출
        /// </summary>
        protected virtual void OnDeadState()
        {
            //this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 넉백
        /// </summary>
        /// <param name="source"></param>
        /// <param name="distance"></param>
        public void Knockback(Vector3 source, float distance = 1.5f)
        {
            this.MoveController.KnockBack(source, distance);
        }
    }
}