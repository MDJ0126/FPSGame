using UnityEngine;

namespace FPSGame.Character
{
    public class HitCollider : MonoBehaviour
    {
        private float CRITICAL_RATE = 1.5F;

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

        /// <summary>
        /// 데미지 처리
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="damage"></param>
        public void HitDamage(Character attacker, Vector3 hitPos, float damage)
        {
            if (attacker.TeamNember == Owner.TeamNember) return;    // 같은 팀의 경우 데미지 처리하지 않는다.
            var effect = GameResourceManager.Instance.GetBlood();
            effect.transform.position = hitPos;
            effect.transform.LookAt(attacker.MyTransform.position);
            effect.transform.localScale = Vector3.one;
            effect.gameObject.SetActive(true);
            switch (parts)
            {
                case eParts.Head:
                    this.Owner.HitDamage(damage * CRITICAL_RATE);
                    break;
                case eParts.Body:
                    this.Owner.Knockback(attacker.MyTransform.position);
                    this.Owner.HitDamage(damage);
                    break;
            }
        }
    }
}