using FPSGame.Character;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    #region Inspector

    public Image foreground;

    #endregion

    private Character _character = null;

    private void OnDestroy()
    {
        if (_character != null)
        {
            _character.OnChangedHitPoint -= OnChangedHitPoint;
        }
    }

    /// <summary>
    /// 타겟 캐릭터 세팅
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacter(Character character)
    {
        if (_character != null)
        {
            _character.OnChangedHitPoint -= OnChangedHitPoint;
        }
        _character = character;
        _character.OnChangedHitPoint += OnChangedHitPoint;
        UpdateHpBar(character.Hp, character.characterData.maxHp);
    }

    private void OnChangedHitPoint(Character character)
    {
        UpdateHpBar(character.Hp, character.characterData.maxHp);
    }

    /// <summary>
    /// 체력 게이지 UI 업데이트
    /// </summary>
    /// <param name="currentHp"></param>
    /// <param name="maxHp"></param>
    private void UpdateHpBar(float currentHp, float maxHp)
    {
        float value = currentHp / maxHp;
        foreground.fillAmount = value;
        foreground.color = GameUtils.GetHpColor(value);
    }
}
