using UnityEngine;

namespace FPSGame.Character
{
	public abstract class Character : MonoBehaviour
	{
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

		protected virtual void Awake()
		{
			this.AnimatorController = this.gameObject.AddComponent<AnimatorController>();
            this.MoveController = this.gameObject.AddComponent<MoveController>();
			this.WeaponHandler = this.gameObject.AddComponent<WeaponHandler>();
		}
    }
}