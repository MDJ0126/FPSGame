public enum eScene
{
    Splash = 0,
    Lobby,
    GameRoom,
    Empty,
}

public enum eGender
{
    None = 0,
    Male,
    Female,
}

public enum eLayer : int
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    HitCollider = 3,
    Water = 4,
    UI = 5,
    Map = 6,
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

public enum eParts
{
    None = 0,
    Hat,
    Hair,
    Facemask,
    FaceAcc,
    Beard,
    Scarf,
    Bag,
    Patch,
    Pouch,
}