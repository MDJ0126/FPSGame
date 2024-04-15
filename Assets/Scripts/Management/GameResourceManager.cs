using UnityEngine;
using FPSGame.Projectile;
using FPSGame.Character;

public class GameResourceManager : SingletonBehaviour<GameResourceManager>
{
    #region Inspector

    public ObjectPool bulletPool;
    public ObjectPool bloodPool;
    public ObjectPool humanPool;
    public ObjectPool zombiePool;

    #endregion

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

    public EffectSystem GetBlood()
    {
        return bloodPool.Get<EffectSystem>();
    }

    public T CreateCharacter<T>(eCharacterType type, Vector3 worldPos, float angle = 0f) where T : Character
    {
        T character = null;
        switch (type)
        {
            case eCharacterType.Human:
                character = humanPool.Get<T>();
                break;
            case eCharacterType.Zombie:
                character = zombiePool.Get<T>();
                break;
        }
        character.MyTransform.position = worldPos;
        character.MyTransform.eulerAngles = Vector3.up * angle;
        return character;
    }
}