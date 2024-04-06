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
		/// �ִϸ����� ��Ʈ�ѷ�
		/// </summary>
		public AnimatorController AnimatorController { get; private set; } = null;
        /// <summary>
        /// �̵� ��Ʈ�ѷ�
        /// </summary>
        public MoveController MoveController { get; private set; } = null;
		/// <summary>
		/// ���� �ڵ鷯
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