using UnityEngine;

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
    }
}