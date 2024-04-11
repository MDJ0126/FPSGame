using UnityEngine;

namespace FPSGame.Character
{
    public class HitCollider : MonoBehaviour
    {
        public enum eParts
        {
            None = 0,
            Head,
            Body,
        }

        #region Inspector
        
        public eParts parts = eParts.None;

        #endregion

        public Character Owner { get; private set; } = null;

        private Collider _collider = null;

        private void Awake()
        {
            this.Owner = GetComponentInParent<Character>();
            _collider = GetComponent<Collider>();
        }

        public void HitDamage(float damage)
        {
            this.Owner.HitDamage(damage);
        }
    }
}