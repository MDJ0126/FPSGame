using UnityEngine;

public static class GameUtils
{
    /// <summary>
    /// 체력 색상 가져오기
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Color GetHpColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, value);
    }
}