using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLoggerUI : MonoBehaviour
{
    #region Inspector

    public TextMeshProUGUI text;
    public int maxLine = 10;

    #endregion

    private List<string> _lines = new List<string>();

    /// <summary>
    /// 로그 추가
    /// </summary>
    /// <param name="message"></param>
    public void AddLog(string message)
    {
        if (_lines.Count > maxLine)
            _lines.RemoveAt(0);
        _lines.Add(message);

        this.text.text = string.Empty;
        for (int i = 0; i < _lines.Count; i++)
        {
            this.text.text += _lines[i];
            if (i < _lines.Count - 1)
            {
                this.text.text += "\n";
            }
        }
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void Clear()
    {
        _lines.Clear();
        text.text = string.Empty;
    }
}