namespace FPSGame.Character
{
    public class BotCharacter : Character
    {
        /// <summary>
        /// AI ��Ʈ�ѷ�
        /// </summary>
        public AIComponent AI { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<AIComponent>();
        }
    }
}