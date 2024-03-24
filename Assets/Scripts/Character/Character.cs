using UnityEngine;

namespace FPSGame.Character
{
	public class Character : MonoBehaviour
	{
		#region Inspector

		public Transform aimCenter;
		public Transform aim;

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

		public MoveController MoveController { get; private set; } = null;

		private void Awake()
		{
            this.MoveController = this.gameObject.AddComponent<MoveController>();

			// 임시로 여기에 넣음
			this.gameObject.AddComponent<PlayerInputController>();
		}

		public void UpdateAimRotation(Vector3 rotate)
		{
			Vector3 angle = aimCenter.eulerAngles;
			aimCenter.rotation = Quaternion.Euler(angle.x - rotate.y, angle.y + rotate.x, angle.z);
		}
	}
}