namespace FPSGame.Character
{
    public class ZombieAI : AIComponent
    {
        private void Update()
        {
            if (owner && !owner.IsDead)
            {
                //owner.MoveController.LootAt(GamePlayManager.Instance.player);
                owner.MoveController.MoveTo(GamePlayManager.Instance.player.MyTransform.position);
            }
        }
    }
}