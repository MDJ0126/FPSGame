using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    #region Inspector

    public TextMeshProUGUI text;

    #endregion

    private DateTime _start;

    private void OnEnable()
    {
        _start = GameConfig.NowTime;
        StartCoroutine("TimerProcess");
    }

    private IEnumerator TimerProcess()
    {
        TimeSpan diff = GameConfig.NowTime - _start;
        while (true)
        {
            DateTime time = new DateTime(diff.Ticks);
            text.text = time.ToString("mm:ss");
            yield return YieldInstructionCache.WaitForSeconds(1f);
            diff = GameConfig.NowTime - _start;
        }
    }
}
