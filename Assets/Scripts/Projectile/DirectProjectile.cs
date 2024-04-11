using Unity.Burst.Intrinsics;
using UnityEngine;

namespace FPSGame.Projectile
{
    public class DirectProjectile : Projectile
    {
        #region Inspector

        public float speed = 1f;
        public float maxDistance = 10f;

        #endregion

        protected override void OnUpdate()
        {
            if (Vector3.Distance(startPos, this.MyTransform.position) < maxDistance)
            {
                this.MyTransform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                this.MyTransform.position = direction * maxDistance;
                Finish(null);
            }
        }
    }
}