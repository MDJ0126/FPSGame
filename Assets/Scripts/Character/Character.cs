using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame.Character
{
	public class Character : MonoBehaviour
	{
		#region Inspector



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

			// �ӽ÷� ���⿡ ����
			this.gameObject.AddComponent<PlayerInput>();
		}
	}
}