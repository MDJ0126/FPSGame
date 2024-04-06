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

        public virtual void Run(Vector3 startPos, Vector3 direction, Action onFinished = null)
        {
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

        public void Finish()
        {
            isPlay = false;
            gameObject.SetActive(false);
            onFinished?.Invoke();
        }
    }
}