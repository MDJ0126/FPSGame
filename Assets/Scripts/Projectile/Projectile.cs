using System;
using UnityEngine;

namespace FPSGame.Projectile
{
    public abstract class Projectile : MonoBehaviour
    {
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
        protected Vector3 startPos = Vector3.zero;
        protected Vector3 direction = Vector3.zero;
        protected Action onFinished = null;

        public virtual void Run(Vector3 startPos, Vector3 direction, float spreadRange = 1f, Action onFinished = null)
        {
            // 랜덤 산탄 각도 적용
            Quaternion spreadRotation = Quaternion.Euler(UnityEngine.Random.Range(-spreadRange, spreadRange), UnityEngine.Random.Range(-spreadRange, spreadRange), 0f);
            direction = spreadRotation * direction;

            isPlay = true;
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
                LayerMask ignoreLayer = 1 << (int)eLayer.IgnoreRaycast;
                if (other.gameObject.layer == ~ignoreLayer)
                {
                    Debug.Log(other.gameObject.name);
                }
            }
        }

        public void Finish()
        {
            isPlay = false;
            gameObject.SetActive(false);
            onFinished?.Invoke();
        }
    }
}