public enum eScene
{
    Empty = 0,
    Splash,
    Lobby,
    GameRoom,
}

public enum eLayer : int
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    HitCollider = 3,
    Water = 3,
    UI = 4,
    Map = 5,
}

public enum eWeaponType
{
    None = 0,
    Gun,
}
public enum eProjectileType
{
    None = 0,
    Bullet,
}

public enum eTeam
{
    MyTeam,
    EnemyTeam,
}

public enum eCharacterState
{
    None = 0,
    Idle,
    Fire1,
    Fire2,
    Fire3,
    Dead,
}

public enum eCharacterType
{
    None = 0,
    Human,
    Zombie,
}