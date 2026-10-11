using FPSGame.Character;
using UnityEngine;

public class GameRoomUIController : MonoBehaviour
{
    #region Inspector

    public GameObject aim;
    public GameObject gameOver;

    #endregion

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        aim.SetActive(true);
        gameOver.SetActive(false);
    }

    private void OnEnable()
    {
        GamePlayManager.Instance.myCharacter.OnDead += OnDeadPlayer;
    }

    private void OnDisable()
    {
        if (GamePlayManager.IsLive)
        {
            GamePlayManager.Instance.myCharacter.OnDead -= OnDeadPlayer;
        }
    }

    private void OnDeadPlayer(Character character)
    {
        aim.SetActive(false);
        gameOver.SetActive(true);
    }
}