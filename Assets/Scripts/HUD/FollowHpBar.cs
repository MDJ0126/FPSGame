using FPSGame.Character;
using UnityEngine.UI;

public class FollowHpBar : FollowHUD
{
    public Image foreground;

    private Character _character = null;

    public void SetCharacter(Character character)
    {
        if (_character != null)
        {
            _character.OnChangedHitPoint -= OnChangedHitPoint;
        }
        _character = character;
        _character.OnChangedHitPoint += OnChangedHitPoint;
    }

    private void OnChangedHitPoint(Character character)
    {
        SetFillAmount(character.Hp / character.characterData.maxHp);
    }

    public void SetFillAmount(float value)
    {
        foreground.fillAmount = value;
        foreground.color = GameUtils.GetHpColor(value);
    }
}
