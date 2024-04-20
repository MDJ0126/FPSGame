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
        private Vector3? _lastFramePosition = null;

        public override void Run(Character.Character sender, Vector3 startPos, Vector3 direction, float spreadRange = 1, OnHitCollider onFinished = null)
        {
            // 랜덤 산탄 각도 적용
            Quaternion spreadRotation = Quaternion.Euler(UnityEngine.Random.Range(-spreadRange, spreadRange), UnityEngine.Random.Range(-spreadRange, spreadRange), 0f);
            direction = spreadRotation * direction;

            base.Run(sender, startPos, direction, spreadRange, onFinished);
        }

        protected override void OnUpdate()
        {
            // 이전 프레임과 현재 프레임 사이 보간하여 충돌 감지
            CheckHitFrameInterval();

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

        /// <summary>
        /// 이전 프레임과 현재 프레임 사이 보간하여 충돌 감지
        /// </summary>
        private void CheckHitFrameInterval()
        {
            if (_lastFramePosition != null)
            {
                RaycastHit hit;
                LayerMask layer = 1 << (int)eLayer.HitCollider;
                Vector3 start = _lastFramePosition.Value;
                Vector3 direction = (this.MyTransform.position - _lastFramePosition.Value).normalized;
                float distance = (this.MyTransform.position - _lastFramePosition.Value).magnitude; // == Vector3.Distance(this.MyTransform.position, _lastFramePosition.Value)
                if (Physics.Raycast(start, direction, out hit, distance, layer))
                {
                    // 불렛 자신도 체크되기 때문에 예외 체크
                    if (!hit.collider.gameObject.Equals(this.gameObject))
                    {
                        // 충돌한 물체가 적인 경우 처리
                        if (IsHit(hit.collider))
                        {
                            HitDamage(hit.collider, hit.point);
                        }
                    }
                }

                Debug.DrawRay(start, direction * Vector3.Distance(this.MyTransform.position, _lastFramePosition.Value), Color.red);
            }
            // 현재 프레임의 위치를 이전 프레임의 위치로 업데이트
            _lastFramePosition = this.MyTransform.position;
        }

        protected override void Clear()
        {
            base.Clear();
            _lastFramePosition = null;
        }
    }
}