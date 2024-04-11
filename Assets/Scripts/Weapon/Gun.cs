using System;

namespace FPSGame.Weapon
{
    public abstract class Gun : Weapon
    {
        public override eWeaponType weaponType => eWeaponType.Gun;

        #region Inspector

        public float spreadRange = 2f;
        public float damage = 10f;

        #endregion

        public override void OnFire()
        {
            base.OnFire();
            var now = DateTime.Now;
            if (shotRecordTime.AddSeconds(shotInterval) < now)
            {
                shotRecordTime = now;
                var bullet = GameResourceManager.Instance.Get(eProjectileType.Bullet);
                bullet.Run(shot.position, shot.forward, spreadRange, (hitCollider) =>
                {
                    hitCollider.HitDamage(damage);
                });
            }
        }
    }
}