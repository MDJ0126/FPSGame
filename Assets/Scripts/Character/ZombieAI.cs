using UnityEngine;

namespace FPSGame.Character
{
    public class ZombieAI : AIComponent
    {
        private static float ATTACK_DISTANCE = 1.2f;
        private static float ATTACK_RANGE = 1.6f;

        private void Update()
        {
            if (owner && !owner.IsDead)
            {
                // 공격 중이지 않을 때
                if (owner.WeaponHandler.IsWaitHandAttack == false)
                {
                    var target = owner.DetectTarget.GetDectedCharacter();
                    if (target == null || target.TeamNember == owner.TeamNember)
                    {
                        // 탐지한 적이 없으면 살아있는 적 탐색
                        target = GamePlayManager.Instance.GetAilveCharacter();
                    }

                    if (target)
                    {
                        float distance = Vector3.Distance(target.MyTransform.position, owner.MyTransform.position);
                        if (distance > ATTACK_DISTANCE)
                        {
                            // 거리가 멀면 이동한다.
                            owner.MoveController.MoveTo(target.MyTransform.position);
                        }
                        else
                        {
                            // 사거리 안에 들어오면 멈추고, 공격한다.
                            owner.MoveController.StopMove();
                            owner.MoveController.LookAt(target, 0f);
                            owner.WeaponHandler.Fire(() =>
                            {
                                Vector3 aimCenter = owner.MyTransform.position;
                                aimCenter.y = owner.AimHeight * 0.5f;
                                var hitInfos = Physics.RaycastAll(aimCenter, owner.MyTransform.forward, ATTACK_RANGE);
                                if (hitInfos != null)
                                {
                                    foreach (var hit in hitInfos)
                                    {
                                        HitCollider hitCollider = hit.collider.GetComponent<HitCollider>();
                                        if (hitCollider && owner)
                                        {
                                            hitCollider.HitDamage(owner, hit.point, owner.characterData.damage);
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }
    }
}