using FPSGame.Character;
using System;
using System.Collections;
using UnityEngine;

namespace FPSGame.Weapon
{
    public abstract class Gun : Weapon
    {
        public override eWeaponType weaponType => eWeaponType.Gun;

        #region Inspector

        public Transform shot;
        public GameObject shotEffectLight;
        public float spreadRange = 0.25f;
        public float damage = 10f;

        #endregion

        public override void Fire(FPSGame.Character.Character owner, Action onFire)
        {
            base.Fire(owner, onFire);
            var now = GameConfig.NowTime;
            if (shotRecordTime.AddSeconds(shotInterval) < now)
            {
                float volume = 0.1f;
                if (!(owner is PlayerCharacter)) volume = 0.05f;
                SoundManager.Instance.PlayEffectSound(this.transform.position, eSoundEffect.gun_fire, volume: volume);

                StopCoroutine("FireEffect");
                StartCoroutine("FireEffect");
                shotRecordTime = now;
                var bullet = GameResourceManager.Instance.Get(eProjectileType.Bullet);
                bullet.Run(owner, shot.position, shot.forward, spreadRange, (hitCollider, hitPoint) =>
                {
                    var score = hitCollider.HitDamage(owner, hitPoint, damage);
                    owner.AddScore(score);
                });
            }
        }

        private IEnumerator FireEffect()
        {
            shotEffectLight.SetActive(true);
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            shotEffectLight.SetActive(false);
        }

        protected override void Initialize()
        {
            base.Initialize();
            shotEffectLight.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();
            Debug.DrawRay(shot.position, shot.forward * 10, Color.yellow);
        }
    }
}