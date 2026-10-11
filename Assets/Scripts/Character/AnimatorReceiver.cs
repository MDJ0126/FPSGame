using System;
using UnityEngine;

namespace FPSGame.Character
{
    public class AnimatorReceiver : MonoBehaviour
    {
        public Action onFire = null;

        private void Fire()
        {
            onFire?.Invoke();
        }
    }
}