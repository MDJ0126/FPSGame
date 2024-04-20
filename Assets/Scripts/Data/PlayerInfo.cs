/// <summary>
/// 플레이어 정보
/// </summary>
public class PlayerInfo
{
    public static PlayerInfo DefaultInfo { get; set; } = new PlayerInfo("Nonamed");

    public delegate void OnChangedData(PlayerInfo playerInfo);
    private event OnChangedData _onChangedScore = null;
    public event OnChangedData OnChangedScore
    {
        add
        {
            _onChangedScore -= value;
            _onChangedScore += value;
        }
        remove
        {
            _onChangedScore -= value;
        }
    }

    /// <summary>
    /// 고유 인덱스
    /// </summary>
    public long Idx { get; private set; } = 0;
    /// <summary>
    /// 이름
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// 점수
    /// </summary>
    public long Score { get; private set; } = 0;

    public PlayerInfo()
    {
        this.Idx = PlayerInfoUtils.CreateIDX();
        this.Name = PlayerInfoUtils.GetRandomName();
        this.Score = 0;
    }

    public PlayerInfo(string name)
    {
        this.Idx = PlayerInfoUtils.CreateIDX();
        this.Name = name;
        this.Score = 0;
    }

    /// <summary>
    /// 점수 증가
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        SetScore(this.Score + score);
    }

    /// <summary>
    /// 점수 세팅
    /// </summary>
    /// <param name="score"></param>
    public void SetScore(long score)
    {
        this.Score = score;
        _onChangedScore?.Invoke(this);
    }

    /// <summary>
    /// 유틸리티
    /// </summary>
    private static class PlayerInfoUtils
    {
        private static long IDX_GENERATOR = 0;

        private static string[] _names = { "Taylor", "Emma", "Ava", "Isabella", "Jaxon", "Anthony", "Joshua", "Asher" };

        /// <summary>
        /// 플레이어 IDX 생성
        /// </summary>
        /// <returns></returns>
        public static long CreateIDX() => ++IDX_GENERATOR;

        /// <summary>
        /// 랜덤 캐릭터 이름 가져오기
        /// </summary>
        /// <returns></returns>
        public static string GetRandomName()
        {
            int index = UnityEngine.Random.Range(0, _names.Length);
            return _names[index];

        }
    }
}