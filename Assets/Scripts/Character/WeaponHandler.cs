using FPSGame.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class WeaponHandler : MonoBehaviour
    {
        private Character _owner = null;
        private FPSGame.Weapon.Weapon _weapon;

        private void Start()
        {
            _owner = GetComponent<Character>();
            Equip(GetComponentInChildren<FPSGame.Weapon.Weapon>(true));
        }

        private void LateUpdate()
        {
            if (_weapon == null) return;

            // LootAt
            _weapon.root.LookAt(_owner.aim);

            // Handle
            if (_weapon.leftHandler)
            {
                _owner.leftHand.data.target.position = _weapon.leftHandler.position;
                _owner.leftHand.data.target.rotation = _weapon.leftHandler.rotation;
            }
            if (_weapon.rightHandler)
            {
                _owner.rightHand.data.target.position = _weapon.rightHandler.position;
                _owner.rightHand.data.target.rotation= _weapon.rightHandler.rotation;
            }
        }

        /// <summary>
        /// ���� ���� ó��
        /// </summary>
        /// <param name="weapon"></param>
        private void Equip(FPSGame.Weapon.Weapon weapon)
        {
            if (weapon == null) return;
            this._weapon = weapon;
        }
    }
}