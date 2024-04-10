using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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
                }
            }
        }
    }
}