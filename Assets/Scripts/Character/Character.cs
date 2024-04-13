using System;
using UnityEngine;

namespace FPSGame.Character
{
	public abstract class Character : MonoBehaviour
	{
		public CharacterData characterData;
		public Transform aim;

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
        }

		public virtual void SetTeam(byte teamNember)
		{
			this.TeamNember = teamNember;
        }

		public eTeam GetTeam(byte teamNumber)
		{
			return this.TeamNember == teamNumber ? eTeam.MyTeam : eTeam.EnemyTeam;
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