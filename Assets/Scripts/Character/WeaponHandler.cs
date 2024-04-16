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
        public bool IsWaitHandAttack { get; private set; } = false;

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
            this.Weapon = weapon;
            if (_owner.weaponAim)
                _owner.weaponAim.data.constrainedObject = weapon.shot;
        }

        /// <summary>
        /// 공격
        /// </summary>
        public void Fire(Action onFire = null)
        {
            if (Weapon)
            {
                // 무기가 있는 경우
                Weapon.Fire(_owner, () =>
                {
                    onFire?.Invoke();
                    if (_isPlayer)
                    {
                        GameCameraController.Instance.Shake(eEaseType.EaseInBack, gain: 2f, duration: 0.1f);
                    }
                });
            }
            else
            {
                // 무기가 없는 경우 맨손 공격
                if (!IsWaitHandAttack)
                {
                    IsWaitHandAttack = true;
                    _owner.AnimatorController.Fire(
                    onFire: () =>
                    {
                        onFire?.Invoke();
                    },
                    onFinished: () =>
                    {
                        IsWaitHandAttack = false;
                    });
                }
            }
        }
    }
}