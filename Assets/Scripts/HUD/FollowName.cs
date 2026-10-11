using TMPro;

public class FollowName : FollowHUD
{
    public TextMeshProUGUI textMeshPro;

    public void SetName(string name)
    {
        textMeshPro.text = name;
    }
}