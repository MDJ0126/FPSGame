using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace FPSGame.Projectile
{
    public class DirectionalProjectile : Projectile
    {
        #region Inspector

        public float speed = 1f;
        public float maxDistance = 10f;

        #endregion

        /// <summary>
        /// 이전 프레임의 위치
        /// </summary>
        private Vector3 _lastFramePosition; 

        public override void Run(Character.Character sender, Vector3 startPos, Vector3 direction, float spreadRange = 1, OnHitCollider onFinished = null)
        {
            // 랜덤 산탄 각도 적용
            Quaternion spreadRotation = Quaternion.Euler(UnityEngine.Random.Range(-spreadRange, spreadRange), UnityEngine.Random.Range(-spreadRange, spreadRange), 0f);
            direction = spreadRotation * direction;

            base.Run(sender, startPos, direction, spreadRange, onFinished);
        }

        protected override void OnUpdate()
        {
            // 이전 프레임과 현재 프레임 사이에 보간하여 충돌을 감지한다.
            RaycastHit hit;
            LayerMask ignoreLayer = 1 << (int)eLayer.IgnoreRaycast | 1 << (int)eLayer.Map;
            if (Physics.Raycast(_lastFramePosition, (this.MyTransform.position - _lastFramePosition).normalized, out hit, Vector3.Distance(this.MyTransform.position, _lastFramePosition), ~ignoreLayer))
            {
                // 충돌한 물체가 적인 경우 처리
                if (IsHit(hit.collider))
                {
                    HitDamage(hit.collider, hit.point);
                }
            }

            Debug.DrawRay(_lastFramePosition, (this.MyTransform.position - _lastFramePosition).normalized * Vector3.Distance(this.MyTransform.position, _lastFramePosition), Color.red);

            // 현재 프레임의 위치를 이전 프레임의 위치로 업데이트
            _lastFramePosition = this.MyTransform.position;

            if (Vector3.Distance(startPos, this.MyTransform.position) < maxDistance)
            {
                this.MyTransform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                this.MyTransform.position = direction * maxDistance;
                HitDamage(null);
            }
        }
    }
}