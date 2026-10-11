using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace FPSGame.Character
{
    public class BotAI : AIComponent
    {
        private const float FORCE_MOVE_DISTANCE = 10f;
        private const float TEAM_DEST_STOPPED_DISTANCE = 6f;
        private const float BACK_MOVE_DISTANCE = 2f;

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
                    owner.MoveController.MoveTo(_player.MyTransform.position, 1.5f);
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
                            // 1. 적을 향해 에임 세팅
                            owner.SetAim(target);

                            // 2. 발사 (적이 장애물을 두고 있지 않으면 발사)
                            Vector3 aimCenter = owner.MyTransform.position;
                            aimCenter.y = owner.AimHeight;
                            Vector3 targetCenter = target.MyTransform.position;
                            targetCenter.y = Character.CHARACTER_HEIGHT_CENTER;

                            Vector3 direction = (targetCenter - aimCenter).normalized;
                            LayerMask ignoreLayer = 1 << (int)eLayer.IgnoreRaycast;
                            if (Physics.Raycast(aimCenter, direction, out var hit, owner.DetectTarget.DetectDistance, ~ignoreLayer))
                            {
                                owner.WeaponHandler.Fire();
                            }

                            // 3. 이동
                            float distance = Vector3.Distance(target.MyTransform.position, owner.MyTransform.position);
                            if (distance < BACK_MOVE_DISTANCE)
                            {
                                // 적이 너무 가까울 경우 빽무빙
                                Vector3 moveDirection = (target.MyTransform.position - owner.MyTransform.position).normalized;
                                owner.MoveController.MoveTo(moveDirection);
                            }
                            else
                            {
                                // 적을 추격
                                owner.MoveController.MoveTo(target.MyTransform.position, 0.5f);
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