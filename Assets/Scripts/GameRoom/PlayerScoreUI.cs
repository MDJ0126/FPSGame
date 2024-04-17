using FPSGame.Character;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
	private readonly string FORM = "Score: {0}";

	#region Inspector

	public TextMeshProUGUI text;

	#endregion

	private PlayerInfo _playerInfo = null;

    private void OnDestroy()
    {
        if (_playerInfo != null)
        {
            _playerInfo.OnChangedScore -= OnChangedScore;
        }
    }

    /// <summary>
    /// 타겟 플레이어 세팅
    /// </summary>
    /// <param name="target"></param>
    public void SetPlayer(PlayerInfo playerInfo)
	{
		if (_playerInfo != null)
		{
            _playerInfo.OnChangedScore -= OnChangedScore;
        }
        _playerInfo = playerInfo;
        _playerInfo.OnChangedScore += OnChangedScore;
        UpdateScore(playerInfo.Score);
    }

    private void OnChangedScore(PlayerInfo playerInfo)
    {
		UpdateScore(playerInfo.Score);
    }

	/// <summary>
	/// 점수 업데이트
	/// </summary>
	/// <param name="score"></param>
    private void UpdateScore(long score)
	{
		text.text = string.Format(FORM, score.ToString("N0"));
    }
}
