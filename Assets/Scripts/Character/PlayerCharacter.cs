namespace FPSGame.Character
{
    public class PlayerCharacter : Character
    {
        protected override void Awake()
        {
            base.Awake();
            this.gameObject.AddComponent<PlayerInputController>();
        }
    }
}