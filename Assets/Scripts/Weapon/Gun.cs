using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame.Weapon
{
    public abstract class Gun : Weapon
    {
        public override eWeaponType weaponType => eWeaponType.Gun;
    }
}