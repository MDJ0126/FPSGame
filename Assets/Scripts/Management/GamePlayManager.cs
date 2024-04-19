using System;
using System.Collections.Generic;
using FPSGame.Character;
using UnityEngine;

public class GamePlayManager : SingletonBehaviour<GamePlayManager>
{
    private const int BOT_PLAYER_COUNT = 3;

    #region Inspector

    /// <summary>
    /// 플레이어 본인 캐릭터
    /// </summary>
    public Character myCharacter;

    /// <summary>
    /// 플레이어 본인 점수
    /// </summary>
    public PlayerScoreUI myPlayerScoreUI;

    /// <summary>
    /// 플레이어 본인 체력 게이지
    /// </summary>
    public PlayerHpBar myPlayerHpBar;

    /// <summary>
    /// 플레이어 상태 UI
    /// </summary>
    public CharacterStateUI[] characterStateUIs;

    /// <summary>
    /// 게임 로거
    /// </summary>
    public GameLoggerUI gameLoggerUI;

    #endregion

    /// <summary>
    /// 플레이 캐릭터 리스트
    /// Key: 캐릭터 IDX
    /// Value: 플레이어 정보
    /// </summary>
    private Dictionary<long, Character> _characterDic = new();

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 초기화
    /// </summary>
    private void Initialize()
    {
        // 초기화
        foreach (var character in _characterDic.Values)
        {
            character.gameObject.SetActive(false);
            character.OnSendLog -= OnSendLogFromCharacter;
        }
        _characterDic.Clear();

        // 로거 초기화
        gameLoggerUI.Clear();

        // 플레이어 상태 UI 비활성화
        foreach (var playerStateUI in characterStateUIs)
        {
            playerStateUI.gameObject.SetActive(false);
        }

        // 내 캐릭터 세팅
        PlayerInfo myPlayerInfo = new PlayerInfo("Player");
        myCharacter.SetPlayerInfo(myPlayerInfo);
        AddCharacter(myCharacter);

        // 봇 캐릭터 세팅
        for (int i = 0; i < BOT_PLAYER_COUNT; i++)
        {
            SummonBotPlayer(myCharacter.MyTransform.position);
        }

        // 점수 세팅
        myPlayerScoreUI.SetPlayer(myCharacter.PlayerInfo);

        // 체력 게이지 세팅
        myPlayerHpBar.SetCharacter(myCharacter);
    }

    /// <summary>
    /// 봇 플레이어 생성
    /// </summary>
    /// <param name="worldPos">생성 위치</param>
    /// <returns></returns>
    public Character SummonBotPlayer(Vector3 worldPos)
    {
        // 캐릭터 생성
        Vector3 randomVector = UnityEngine.Random.insideUnitSphere;
        var pos = worldPos + new Vector3(randomVector.x, 0f, randomVector.z);
        var bot = GameResourceManager.Instance.CreateCharacter<BotCharacter>(eCharacterType.Human, pos);
        bot.gameObject.SetActive(true);
        bot.Initiailize();

        // 정보 세팅
        var botPlayerInfo = new PlayerInfo();
        bot.SetPlayerInfo(botPlayerInfo);
        AddCharacter(bot);

        return bot;
    }

    /// <summary>
    /// 플레이어 추가
    /// </summary>
    /// <param name="character"></param>
    public void AddCharacter(Character character)
    {
        if (_characterDic.ContainsKey(character.PlayerInfo.Idx)) return;
        _characterDic.Add(character.PlayerInfo.Idx, character);
        character.OnSendLog += OnSendLogFromCharacter;

        // 플레이어 본인이 아닌 경우 UI 등록
        if (!myCharacter.Equals(character))
        {
            var stateUI = Array.Find(characterStateUIs, characterStateUI => !characterStateUI.gameObject.activeSelf);
            if (stateUI != null)
            {
                stateUI.gameObject.SetActive(true);
                stateUI.SetCharacter(character);
            }
        }

        // HUD 등록
        if (!myCharacter.Equals(character))
            GameHUDManager.Instance.SetFollowName(character);

        // 참여 로그
        AddLog($"{character.PlayerInfo.Name}(이)가 참여했습니다.");
    }

    /// <summary>
    /// 살아있는 아무 캐릭터 가져오기
    /// </summary>
    /// <returns></returns>
    public Character GetAilveCharacter()
    {
        Character ailveCharacter = null;
        foreach (var character in _characterDic.Values)
        {
            if (!character.IsDead)
            {
                ailveCharacter = character;
                break;
            }
        }
        return ailveCharacter;
    }

    /// <summary>
    /// 로그 남기기
    /// </summary>
    /// <param name="message"></param>
    private void OnSendLogFromCharacter(string message)
    {
        AddLog(message);
    }

    /// <summary>
    /// 플레이어 제거
    /// </summary>
    /// <param name="idx"></param>
    public void RemoveCharacter(long idx)
    {
        if (_characterDic.TryGetValue(idx, out var character))
        {
            _characterDic.Remove(idx);
            character.OnSendLog -= OnSendLogFromCharacter;

            // 플레이어 본인이 아닌 경우 UI 등록 해제
            if (!myCharacter.Equals(character))
            {
                var stateUI = Array.Find(characterStateUIs, characterStateUI => !characterStateUI.Character.Equals(character));
                if (stateUI != null)
                {
                    stateUI.gameObject.SetActive(false);
                    stateUI.SetCharacter(null);
                }
            }
        }
    }

    /// <summary>
    /// 로그 추가
    /// </summary>
    /// <param name="message"></param>
    public void AddLog(string message)
    {
        gameLoggerUI.AddLog(message);
    }
}