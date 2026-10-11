using FPSGame.Character;

public class GameHUDManager : SingletonBehaviour<GameHUDManager>
{
    #region Inspector

    public ObjectPool followNamePool;
    public ObjectPool followHpBarPool;

    #endregion

    public void SetFollowName(Character character)
    {
        const float HEIGHT_OFFSET = 20f;
        FollowName followName = followNamePool.Get<FollowName>();
        if (followName)
        {
            followName.SetTarget(character.hudPos);
            followName.SetHeight(HEIGHT_OFFSET);
            followName.SetName(character.PlayerInfo.Name);
            followName.gameObject.SetActive(true);
        }
    }

    public void SetFollowHpBar(Character character)
    {
        const float HEIGHT_OFFSET = 5f;
        FollowHpBar followHpBar = followHpBarPool.Get<FollowHpBar>();
        if (followHpBar)
        {
            followHpBar.SetTarget(character.hudPos);
            followHpBar.SetHeight(HEIGHT_OFFSET);
            followHpBar.SetCharacter(character);
            followHpBar.gameObject.SetActive(true);
        }
    }
}
