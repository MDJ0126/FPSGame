namespace FPSGame.Character
{
    public class BotCharacter : Character
    {
        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<BotAI>();
        }
    }
}