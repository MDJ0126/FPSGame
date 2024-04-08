using UnityEngine;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class WeaponHandler : MonoBehaviour
    {
        private Character _owner = null;
        public FPSGame.Weapon.Weapon Weapon { get; private set; } = null;

        private void Start()
        {
            _owner = GetComponent<Character>();
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
        }

        public void Fire()
        {
            Weapon.OnFire();
        }
    }
}