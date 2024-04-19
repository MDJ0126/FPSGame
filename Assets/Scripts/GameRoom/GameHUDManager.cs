using FPSGame.Character;

public class GameHUDManager : SingletonBehaviour<GameHUDManager>
{
    #region Inspector

    public ObjectPool followNamePool;

    #endregion

    public void SetFollowName(Character character)
    {
        FollowName followName = followNamePool.Get<FollowName>();
        if (followName)
        {
            followName.SetTarget(character.nameHUDPos);
            followName.SetName(character.PlayerInfo.Name);
            followName.gameObject.SetActive(true);
        }
    }
}
