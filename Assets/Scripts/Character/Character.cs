using System;
using UnityEngine;

namespace FPSGame.Character
{
	public abstract class Character : MonoBehaviour
    {
        public static float CHARACTER_HEIGHT_CENTER = 1f;

        public CharacterData characterData;
		public Transform aim;

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

		private float _aimHeight = 0f;
		private float _aimDistance = 0f;


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

            this.Hp = characterData.maxHp;

			_aimHeight = aim.localPosition.y;
            _aimDistance = aim.localPosition.z;
        }

		public virtual void SetTeam(byte teamNember)
		{
			this.TeamNember = teamNember;
        }

		public eTeam GetTeam(byte teamNumber)
		{
			return this.TeamNember == teamNumber ? eTeam.MyTeam : eTeam.EnemyTeam;
        }

		public void SetAim(Character target)
		{
			if (target != null)
			{
				Vector3 aimCenter = this.MyTransform.position;
				aimCenter.y = _aimHeight;
				Vector3 targetCenter = target.MyTransform.position;
				targetCenter.y = CHARACTER_HEIGHT_CENTER;
				Vector3 direction = targetCenter - aimCenter;
                aim.position = aimCenter + direction * _aimDistance;
            }
		}

		public void HitDamage(float damage)
		{
			if (this.Hp > 0f)
			{
				this.Hp -= damage;
				if (this.Hp <= 0f)
				{
					Dead();
				}
			}
		}

        public void SetState(eCharacterState state, Action onFinished = null)
		{
			_onChangeCharacterState?.Invoke(state);
            this.AnimatorController.SetState(state, onFinished);
        }

        public void Dead()
        {
            SetState(eCharacterState.Dead, () =>
            {
                this.gameObject.SetActive(false);
            });
        }
    }
}