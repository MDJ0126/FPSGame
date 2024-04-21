using FPSGame.Character;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame.Projectile
{
    public abstract class Projectile : MonoBehaviour
    {
        public delegate void OnHitCollider(HitCollider hitCollider, Vector3 hitPoint);

        private Transform _myTransform = null;
        public Transform MyTransform
        {
            get
            {
                if (_myTransform == null)
                    _myTransform = transform;
                return _myTransform;
            }
        }

        protected bool isPlay = false;
        protected FPSGame.Character.Character sender = null;
        protected Vector3 startPos = Vector3.zero;
        protected Vector3 direction = Vector3.zero;
        protected OnHitCollider onFinished = null;
        private List<FPSGame.Character.Character> _hitList = new();

        public virtual void Run(FPSGame.Character.Character sender, Vector3 startPos, Vector3 direction, float spreadRange = 1f, OnHitCollider onFinished = null)
        {
            Clear();
            isPlay = true;
            this.sender = sender;
            this.MyTransform.position = startPos;
            this.MyTransform.LookAt(startPos + direction);
            this.startPos = startPos;
            this.direction = direction;
            this.onFinished = onFinished;
            this.gameObject.SetActive(true);
        }

        protected abstract void OnUpdate();

        protected virtual void Clear()
        {
            _hitList.Clear();
        }

        private void LateUpdate()
        {
            if (isPlay) OnUpdate();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (IsHit(other))
            {
                HitDamage(other, this.MyTransform.position);
            }
        }

        protected bool IsHit(Collider collider)
        {
            if (collider)
            {
                if ((collider.gameObject.layer & (int)eLayer.HitCollider) != 0)
                {
                    var hitCollider = collider.GetComponent<HitCollider>();
                    if (hitCollider && !hitCollider.Owner.IsDead)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void HitDamage(Collider collider, Vector3 hitPoint = default)
        {
            if (collider)
            {
                var hitCollider = collider.GetComponent<HitCollider>();
                if (hitCollider && !sender.Equals(hitCollider.Owner))
                {
                    bool isEnemy = hitCollider.Owner.TeamNember != sender.TeamNember;
                    bool isSenderPlayer = sender is PlayerCharacter;    // 플레이어면 아무나 타격 가능하도록
                    if (isEnemy || isSenderPlayer)
                    {
                        if (!hitCollider.Owner.IsDead)
                        {
                            if (!_hitList.Exists(h => h.Equals(hitCollider.Owner)))
                            {
                                _hitList.Add(hitCollider.Owner);
                                isPlay = false;
                                gameObject.SetActive(false);
                                var temp = onFinished;
                                onFinished = null;
                                if (hitCollider != null)
                                    temp?.Invoke(hitCollider, hitPoint);
                            }
                        }
                    }
                }
            }
        }
    }
}