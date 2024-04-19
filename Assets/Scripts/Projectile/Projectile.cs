﻿using FPSGame.Character;
using System.Collections.Generic;
using UnityEngine;
using static FPSGame.Projectile.Projectile;

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
            isPlay = true;
            _hitList.Clear();
            this.sender = sender;
            this.MyTransform.position = startPos;
            this.MyTransform.LookAt(startPos + direction);
            this.startPos = startPos;
            this.direction = direction;
            this.onFinished = onFinished;
            this.gameObject.SetActive(true);
        }

        protected abstract void OnUpdate();

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
                    return true;
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
                    if (isEnemy || sender is PlayerCharacter)
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