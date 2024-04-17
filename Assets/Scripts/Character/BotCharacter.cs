using UnityEngine;

namespace FPSGame.Character
{
    public class BotCharacter : AICharacter
    {
        #region Inspector

        [Header("Model")]
        public GameObject selectModel;
        public GameObject[] models;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<BotAI>();
            SetTeam(GameConfig.MYTEAM_NUMBER);
        }

        public override void Initiailize()
        {
            base.Initiailize();
            if (selectModel) selectModel.SetActive(false);
            int randomIndex = Random.Range(0, models.Length);
            selectModel = models[randomIndex];
            selectModel.SetActive(true);
        }

        public override void Dead(Character attacker)
        {
            base.Dead(attacker);
            SendLog($"{PlayerInfo.Name}(이)가 사망했습니다.");
        }
    }
}