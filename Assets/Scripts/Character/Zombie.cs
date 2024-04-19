﻿using UnityEngine;

namespace FPSGame.Character
{
    public class Zombie : AICharacter
    {
        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<ZombieAI>();
            SetTeam(GameConfig.ENEMYTEAM_NUMBER);
        }

        public override void Dead(Character attacker)
        {
            base.Dead(attacker);

            if (attacker)
            {
                int killScore = 100;
                attacker.AddScore(killScore);
                if (attacker.PlayerInfo != null)
                    attacker.SendLog($"{attacker.PlayerInfo.Name}(이)가 좀비를 처치했습니다! (+{killScore})");
            }
        }

        protected override void OnDeadState()
        {
            base.OnDeadState();
            this.gameObject.SetActive(false);
        }
    }
}