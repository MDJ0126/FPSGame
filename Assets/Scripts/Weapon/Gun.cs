using System;
using UnityEngine;

namespace FPSGame.Weapon
{
    public abstract class Gun : Weapon
    {
        public override eWeaponType weaponType => eWeaponType.Gun;
        public float spreadRange = 2f;

        public override void OnFire()
        {
            base.OnFire();
            var now = DateTime.Now;
            if (shotRecordTime.AddSeconds(shotInterval) < now)
            {
                shotRecordTime = now;
                var bullet = GameResourceManager.Instance.Get(GameResourceManager.eType.Bullet);
                bullet.Run(shot.position, shot.forward, spreadRange , () =>
                {
                    Debug.Log("OnFinished");
                });
            }
        }
    }
}