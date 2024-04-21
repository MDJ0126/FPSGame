using UnityEngine;

namespace FPSGame.Character
{
    public class BotCharacter : AICharacter
    {
        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<BotAI>();
            SetTeam(GameConfig.MYTEAM_NUMBER);
        }

        public override void Dead(Character attacker)
        {
            base.Dead(attacker);
            SendLog($"{PlayerInfo.Name}(이)가 사망했습니다.");
        }

        protected override void OnDeadState()
        {
            base.OnDeadState();
        }
    }
}