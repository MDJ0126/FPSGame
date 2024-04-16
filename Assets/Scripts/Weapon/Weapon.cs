using System;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace FPSGame.Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        public delegate void OnFireEvent();

        public static string SHOT_HANDLER_NAME = "Shot";
        public static string LEFT_HANDLER_NAME = "LeftHandler";
        public static string RIGHT_HANDLER_NAME = "RightHandler";

        public virtual eWeaponType weaponType => eWeaponType.None;

        #region Inspector

        [Header("Transforms")]
        public Transform shot;
        public Transform leftHandler;
        public Transform rightHandler;

        [Header("Variables")]
        public float shotInterval = 1f;

        #endregion

        protected DateTime shotRecordTime;

        public virtual void Fire(FPSGame.Character.Character owner, Action onFire = null) 
        {
            onFire?.Invoke();
        }

        private void Update()
        {
            Debug.DrawRay(shot.position, shot.forward * 10, Color.yellow);
        }

#if UNITY_EDITOR
        [ContextMenu("Auto Setting")]
        private void AutoSetting()
        {
            var childs = this.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < childs.Length; i++)
            {
                Transform child = childs[i];
                if (child.name.Equals(SHOT_HANDLER_NAME))
                {
                    shot = child;
                }
                else if (child.name.Equals(LEFT_HANDLER_NAME))
                {
                    leftHandler = child;
                }
                else if (child.name.Equals(RIGHT_HANDLER_NAME))
                {
                    rightHandler = child;
                }
            }
        }
#endif
    }
}