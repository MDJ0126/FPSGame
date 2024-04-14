using UnityEngine;

namespace FPSGame.Character
{
    public class BotAI : AIComponent
    {
        private void Update()
        {
            if (owner && !owner.IsDead)
            {
                var target = owner.DetectTarget.GetDectedCharacter();
                if (target)
                {
                    owner.MoveController.LootAt(target);
                    if (target.TeamNember != owner.TeamNember)
                    {
                        owner.WeaponHandler.Fire();
                        owner.SetAim(target);
                    }
                    else
                    {
                        bool isMove = Vector3.Distance(target.MyTransform.position, owner.MyTransform.position) > 2f;
                        if (isMove)
                        {
                            owner.MoveController.MoveTo(target.MyTransform.position);
                        }
                        else
                        {
                            owner.MoveController.StopMove();
                        }
                    }
                }
            }
        }
    }
}