using System;
using UnityEngine;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class WeaponHandler : MonoBehaviour
    {
        private Character _owner = null;
        private bool _isPlayer = false;
        public FPSGame.Weapon.Weapon Weapon { get; private set; } = null;

        private void Start()
        {
            _owner = GetComponent<Character>();
            _isPlayer = _owner as PlayerCharacter;
            Equip(GetComponentInChildren<FPSGame.Weapon.Weapon>(true));
        }

        private void LateUpdate()
        {
            if (Weapon == null) return;

            // Update Handler
            if (Weapon.leftHandler)
            {
                _owner.AnimatorController.LeftHand.data.target.rotation = Weapon.leftHandler.rotation;
                _owner.AnimatorController.LeftHand.data.target.position = Weapon.leftHandler.position;
            }
        }

        /// <summary>
        /// 무기 장착 처리
        /// </summary>
        /// <param name="weapon"></param>
        private void Equip(FPSGame.Weapon.Weapon weapon)
        {
            if (weapon == null) return;
            if (this.Weapon != null)
            {
                this.Weapon.OnFire -= OnFireCallback;
            }
            this.Weapon = weapon;
            this.Weapon.OnFire += OnFireCallback;
        }

        /// <summary>
        /// 공격 실행 콜백
        /// </summary>
        private void OnFireCallback()
        {
            if (_isPlayer)
            {
                GameCameraController.Instance.Shake(eEaseType.EaseInBack, gain: 2f, duration: 0.1f);
            }
        }

        /// <summary>
        /// 공격
        /// </summary>
        public void Fire()
        {
            Weapon?.Fire(_owner);
        }
    }
}