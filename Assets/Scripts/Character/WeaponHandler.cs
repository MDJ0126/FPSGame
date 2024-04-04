using FPSGame.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private eWeaponType weaponType = eWeaponType.None;

        private Character _owner = null;
        private FPSGame.Weapon.Weapon _weapon;

        private void Start()
        {
            _owner = GetComponent<Character>();
            Equip(GetComponentInChildren<FPSGame.Weapon.Weapon>(true));
        }

        private void Update()
        {
            if (_weapon == null) return;

            // LootAt
            _weapon.root.LookAt(_owner.aim);

            // Handle
            _owner.leftHand.data.target.position = _weapon.leftHandler.position;
            _owner.rightHand.data.target.position = _weapon.rightHandler.position;
        }

        private void Equip(FPSGame.Weapon.Weapon weapon)
        {
            if (weapon == null) return;
            this._weapon = weapon;
        }
    }
}