using UnityEngine;

namespace FPSGame.Character
{
    public class BotAI : AIComponent
    {
        private const float FORCE_MOVE_DISTANCE = 6f;
        private const float TEAM_DEST_STOPPED_DISTANCE = 3f;
        private const float ENEMY_BACK_MOVE_INTERVAL = 4f;

        private Character _player = null;

        protected override void Awake()
        {
            base.Awake();
            _player = GamePlayManager.Instance.myCharacter;
        }

        private void Update()
        {
            if (owner && !owner.IsDead)
            {
                bool isForceMove = Vector3.Distance(_player.MyTransform.position, owner.MyTransform.position) > FORCE_MOVE_DISTANCE;
                if (isForceMove)
                {
                    // 거리가 너무 이격되면 플레이어에게 이동
                    owner.MoveController.MoveTo(_player.MyTransform.position);
                }
                else
                {
                    var target = owner.DetectTarget.GetDectedCharacter();
                    if (target == null || target.TeamNember == owner.TeamNember)
                    {
                        // 탐지한 적이 없으면 플레이어를 추적
                        target = _player;
                    }

                    if (target)
                    {
                        owner.MoveController.LookAt(target);
                        bool isEnemy = target.TeamNember != owner.TeamNember;
                        if (isEnemy)
                        {
                            // 적을 향해 에임 세팅
                            owner.SetAim(target);

                            // 발사
                            owner.WeaponHandler.Fire();

                            // 뒷걸음 처리
                            bool isMove = Vector3.Distance(target.MyTransform.position, owner.MyTransform.position) < ENEMY_BACK_MOVE_INTERVAL;    // 일정 영역 안에 들어오면 뒷걸음 판단
                            //bool isMove = true; // 일단 무조건 빽무빙하려는 경우
                            if (isMove)
                            {
                                Vector3 direction = (target.MyTransform.position - owner.MyTransform.position).normalized;
                                owner.MoveController.MoveTo(direction);
                            }
                        }
                        else
                        {
                            bool isMove = Vector3.Distance(target.MyTransform.position, owner.MyTransform.position) > TEAM_DEST_STOPPED_DISTANCE;
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
}