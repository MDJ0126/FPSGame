using FPSGame.Character;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateUI : MonoBehaviour
{
	#region Inspector

	public Image thumbnail;
    public GameObject dead;
	public Image hpBarForeground;
	public TextMeshProUGUI characterName;
    public TextMeshProUGUI scoreText;

	#endregion

	public Character Character { get; private set; } = null;

    private void OnDestroy()
    {
        if (this.Character != null)
        {
            this.Character.OnChangedHitPoint -= OnChangedHitPoint;
            this.Character.PlayerInfo.OnChangedScore -= OnChangedScore;
        }
    }

    private void OnChangedScore(PlayerInfo playerInfo)
    {
        UpdateScore(playerInfo.Score);
    }

    private void OnChangedHitPoint(Character character)
    {
        UpdateHpBar(character.Hp, character.characterData.maxHp);
    }

    /// <summary>
    /// 캐릭터 세팅
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacter(Character character)
	{
        if (this.Character != null)
        {
            this.Character.OnChangedHitPoint -= OnChangedHitPoint;
            this.Character.PlayerInfo.OnChangedScore -= OnChangedScore;
        }
        this.Character = character;
        if (this.Character != null)
        {
            this.Character.OnChangedHitPoint += OnChangedHitPoint;
            this.Character.PlayerInfo.OnChangedScore += OnChangedScore;
            characterName.text = character.PlayerInfo.Name;
            SetThumbnail(character);
            UpdateHpBar(character.Hp, character.characterData.maxHp);
            UpdateScore(character.PlayerInfo.Score);
        }
    }

	private void SetThumbnail(Character character)
	{
        // todo..
    }

    /// <summary>
    /// 체력 게이지 UI 업데이트
    /// </summary>
    /// <param name="currentHp"></param>
    /// <param name="maxHp"></param>
    private void UpdateHpBar(float currentHp, float maxHp)
    {
        float value = currentHp / maxHp;
        hpBarForeground.fillAmount = value;
        dead.SetActive(value <= 0f);
    }

    /// <summary>
    /// 점수 업데이트
    /// </summary>
    /// <param name="score"></param>
    private void UpdateScore(long score)
    {
        scoreText.text = score.ToString("N0");
    }
}
