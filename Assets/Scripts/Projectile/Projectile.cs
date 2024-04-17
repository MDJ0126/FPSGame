﻿using FPSGame.Character;
using System;
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

        public virtual void Run(FPSGame.Character.Character sender, Vector3 startPos, Vector3 direction, float spreadRange = 1f, OnHitCollider onFinished = null)
        {
            // 랜덤 산탄 각도 적용
            Quaternion spreadRotation = Quaternion.Euler(UnityEngine.Random.Range(-spreadRange, spreadRange), UnityEngine.Random.Range(-spreadRange, spreadRange), 0f);
            direction = spreadRotation * direction;

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

        private void LateUpdate()
        {
            if (isPlay) OnUpdate();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other)
            {
                if ((other.gameObject.layer & (int)eLayer.HitCollider) != 0)
                {
                    var hitCollider = other.GetComponent<HitCollider>();
                    if (hitCollider && sender)
                    {
                        bool isEnemy = hitCollider.Owner.TeamNember != sender.TeamNember;
                        if (isEnemy)
                        {
                            if (!hitCollider.Owner.IsDead)
                            {
                                Finish(hitCollider, this.MyTransform.position);
                            }
                        }
                    }
                }
            }
        }

        public void Finish(HitCollider hitCollider, Vector3 hitPoint = default)
        {
            isPlay = false;
            gameObject.SetActive(false);
            var temp = onFinished;
            onFinished = null;
            if (hitCollider != null)
                temp?.Invoke(hitCollider, hitPoint);
        }
    }
}