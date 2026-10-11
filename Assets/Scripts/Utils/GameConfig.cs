using System;
using UnityEngine;

public static class GameConfig
{
    /// <summary>
    /// 내 팀 번호
    /// </summary>
    public static readonly byte MYTEAM_NUMBER = 1;
    /// <summary>
    /// 적 팀 번호
    /// </summary>
    public static readonly byte ENEMYTEAM_NUMBER = 2;
    /// <summary>
    /// 시작 시간 캐싱 (기기 시간 조작 방지)
    /// </summary>
    private static DateTime _startedTime = DateTime.Now;
    /// <summary>
    /// 현재 시간
    /// </summary>
    public static DateTime NowTime => _startedTime.AddSeconds(Time.realtimeSinceStartupAsDouble);

    static GameConfig()
    {
        _startedTime = DateTime.Now;
    }
}