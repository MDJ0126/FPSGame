public enum eLayer : int
{
    Default = 0,
    TransparentFX = 1 << 1,
    IgnoreRaycast = 1 << 2,

    Water = 1 << 4,
    UI = 1 << 5,
}

public enum eWeaponType
{
    None = 0,
    Gun,
}