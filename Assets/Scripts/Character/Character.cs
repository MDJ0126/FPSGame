using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace FPSGame.Character
{
	public abstract class Character : MonoBehaviour
	{
		#region Inspector

		public Transform aimCenter;
		public Transform aim;
		public TwoBoneIKConstraint leftHand;
		public TwoBoneIKConstraint rightHand;

		#endregion

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

		public void UpdateAimRotation(Vector3 rotate)
		{
			Vector3 angle = aimCenter.eulerAngles;
			aimCenter.rotation = Quaternion.Euler(angle.x - rotate.y, angle.y + rotate.x, angle.z);
        }

#if UNITY_EDITOR
        [ContextMenu("Auto Setting")]
        private void AutoSetting()
        {
			var childs = this.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < childs.Length; i++)
            {
				Transform child = childs[i];
                if (child.name.Equals("Aim Center"))
                {
                    aimCenter = child;
                }
                else if (child.name.Equals("Aim"))
                {
                    aim = child;
                }
                else if (child.name.Equals("LeftArm"))
                {
                    leftHand = child.GetComponent<TwoBoneIKConstraint>();
                }
                else if (child.name.Equals("RightArm"))
                {
                    rightHand = child.GetComponent<TwoBoneIKConstraint>();
                }
            }
        }
#endif
    }
}