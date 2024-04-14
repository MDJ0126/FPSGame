using UnityEngine;

namespace FPSGame.Character
{
    public class BotAI : AIComponent
    {
        private const float TEAM_INTERVAL = 3f;
        private const float SAFETY_INTERVAL = 4f;

        private void Update()
        {
            if (owner && !owner.IsDead)
            {
                var target = owner.DetectTarget.GetDectedCharacter();
                if (target)
                {
                    owner.MoveController.LootAt(target);
                    bool isEnemy = target.TeamNember != owner.TeamNember;
                    if (isEnemy)
                    {
                        // 적을 향해 에임 세팅
                        owner.SetAim(target);

                        // 발사
                        owner.WeaponHandler.Fire();

                        // 뒷걸음 처리
                        //bool isMove = Vector3.Distance(target.MyTransform.position, owner.MyTransform.position) < SAFETY_INTERVAL;    // 일정 영역 안에 들어오면 뒷걸음 판단
                        bool isMove = true; // 일단 무조건 움직이게 함 (테스트해보니 생각보다 움직임이 자연스러워져서)
                        if (isMove)
                        {
                            Vector3 direction = (target.MyTransform.position - owner.MyTransform.position).normalized;
                            owner.MoveController.MoveTo(direction); 
                        }
                    }
                    else
                    {
                        bool isMove = Vector3.Distance(target.MyTransform.position, owner.MyTransform.position) > TEAM_INTERVAL;
                        if (isMove)
                        {
                            // 아군에게 이동
                            owner.MoveController.MoveTo(target.MyTransform.position);
                        }
                        else
                        {
                            // 적당한 거리로 오면 정지
                            owner.MoveController.StopMove();
                        }
                    }
                }
            }
        }
    }
}