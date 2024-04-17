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
        public int HitDamage(Character attacker, Vector3 hitPos, float damage)
        {
            if (attacker.TeamNember == Owner.TeamNember)
            {
                // 같은 팀의 경우 데미지 처리하지 않는다. (플레이어 본인만)
                if (!(attacker is PlayerCharacter)) return 0;
            }
            var effect = GameResourceManager.Instance.GetBlood();
            effect.transform.position = hitPos;
            effect.transform.LookAt(attacker.MyTransform.position);
            effect.transform.localScale = Vector3.one;
            effect.gameObject.SetActive(true);
            switch (parts)
            {
                case eParts.Head:
                    this.Owner.HitDamage(attacker, damage * CRITICAL_RATE);
                    break;
                case eParts.Body:
                    this.Owner.Knockback(attacker.MyTransform.position);
                    this.Owner.HitDamage(attacker, damage);
                    break;
            }
            return GetScore(parts);
        }

        /// <summary>
        /// 부위별 점수
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        protected virtual int GetScore(eParts parts)
        {
            switch (parts)
            {
                case eParts.Head:
                    return 50;
                case eParts.Body:
                    return 10;
                default:
                    return 5;
            }
        }
    }
}