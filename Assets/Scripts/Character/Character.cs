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
		public float Hp = 100f;
		/// <summary>
		/// 사망 여부
		/// </summary>
		public bool IsDead => Hp <= 0f;


        protected virtual void Awake()
		{
			this.AnimatorController = this.gameObject.AddComponent<AnimatorController>();
            this.MoveController = this.gameObject.AddComponent<MoveController>();
			this.WeaponHandler = this.gameObject.AddComponent<WeaponHandler>();
			this.DetectTarget = DetectTarget.AddDetectTarget(this);

            this.Hp = characterData.maxHp;
        }

		public virtual void SetData(byte teamNember)
		{
			this.TeamNember = teamNember;
        }

		public eTeam GetTeam(byte teamNumber)
		{
			return this.TeamNember == teamNumber ? eTeam.MyTeam : eTeam.EnemyTeam;
		}
    }
}