namespace FPSGame.Character
{
    public class BotCharacter : Character
    {
        /// <summary>
        /// AI 컨트롤러
        /// </summary>
        public AIComponent AI { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<AIComponent>();
        }
    }
}