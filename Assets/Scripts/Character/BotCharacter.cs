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
    }
}