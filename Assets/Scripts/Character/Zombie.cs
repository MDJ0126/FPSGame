using UnityEngine;

namespace FPSGame.Character
{
    public class Zombie : AICharacter
    {
        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<ZombieAI>();
        }

        private void Update()
        {
            this.MoveController.LootAt(GamePlayManager.Instance.player);
            this.MoveController.MoveTo(GamePlayManager.Instance.player.MyTransform.position);
        }
    }
}