using FPSGame.Projectile;

public class GameResourceManager : SingletonBehaviour<GameResourceManager>
{
    public enum eType
    {
        None = 0,
        Bullet,
    }

    #region Inspector

    public ObjectPool bulletPool;

    #endregion

    public Projectile Get(eType type)
    {
        Projectile projectile = null;
        switch (type)
        {
            case eType.None:
                break;
            case eType.Bullet:
                projectile = bulletPool.Get<Projectile>();
                break;
            default:
                break;
        }
        return projectile;
    }
}