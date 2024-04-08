using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPSGame.Character
{
    [RequireComponent(typeof(BotCharacter))]
    public class AIComponent : MonoBehaviour
    {
        private Character _owner = null;

        private void Start()
        {
            _owner  = GetComponent<Character>();
        }

        private void Update()
        {
            if (_owner && !_owner.IsDead)
            {
                var target = _owner.DetectTarget.GetDectedCharacter();
                if (target)
                {
                    _owner.MoveController.LootAt(target);
                }
            }
        }
    }
}