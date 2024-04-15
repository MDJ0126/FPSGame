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

        public override void Fire(FPSGame.Character.Character owner, Action onFire)
        {
            base.Fire(owner, onFire);
            var now = DateTime.Now;
            if (shotRecordTime.AddSeconds(shotInterval) < now)
            {
                shotRecordTime = now;
                var bullet = GameResourceManager.Instance.Get(eProjectileType.Bullet);
                bullet.Run(shot.position, shot.forward, spreadRange, (hitCollider, hitPoint) =>
                {
                    hitCollider.HitDamage(owner, hitPoint, damage);
                });
            }
        }
    }
}