using UnityEngine;
using FPSGame.Projectile;
using FPSGame.Character;

public class GameResourceManager : SingletonBehaviour<GameResourceManager>
{
    #region Inspector

    public ObjectPool bulletPool;
    public ObjectPool characterPool;

    #endregion

    private void Awake()
    {
        var bot = CreateBotCharacter(Vector3.zero, 180f, 2);
        bot.gameObject.SetActive(true);
    }

    public Projectile Get(eProjectileType type)
    {
        Projectile projectile = null;
        switch (type)
        {
            case eProjectileType.None:
                break;
            case eProjectileType.Bullet:
                projectile = bulletPool.Get<Projectile>();
                break;
            default:
                break;
        }
        return projectile;
    }

    public BotCharacter CreateBotCharacter(Vector3 worldPos, float angle, byte teamNumber)
    {
        BotCharacter bot = characterPool.Get<BotCharacter>();
        bot.SetTeam(teamNumber);
        bot.MyTransform.position = worldPos;
        bot.MyTransform.eulerAngles = Vector3.up * angle;
        return bot;
    }
}