using UnityEngine;

namespace FPSGame.Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        public static string ROOT_HANDLER_NAME = "Root";
        public static string LEFT_HANDLER_NAME = "LeftHandler";
        public static string RIGHT_HANDLER_NAME = "RightHandler";

        public virtual eWeaponType weaponType => eWeaponType.None;
        public Transform leftHandler = null;
        public Transform rightHandler = null;

#if UNITY_EDITOR
        [ContextMenu("Auto Setting")]
        private void AutoSetting()
        {
            var childs = this.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < childs.Length; i++)
            {
                Transform child = childs[i];
                if (child.name.Equals(LEFT_HANDLER_NAME))
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