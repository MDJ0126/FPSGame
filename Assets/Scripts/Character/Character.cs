using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame.Character
{
	public class Character : MonoBehaviour
	{
		#region Inspector



		#endregion

		public MoveController MoveController { get; private set; } = null;

		private void Awake()
		{
            this.MoveController = this.gameObject.AddComponent<MoveController>();
		}
	}
}