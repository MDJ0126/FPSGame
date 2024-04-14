using FPSGame.Character;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : SingletonBehaviour<SpawnManager>
{
    #region Inspector

    public float createDistance = 15f;
    public float createInterval = 3f;

    #endregion

    private Character _target = null;
    private Coroutine _spawnCoroutine = null;

    private void Start()
    {
        _target = GamePlayManager.Instance.player;
        _spawnCoroutine = StartCoroutine(SpawnProcess());
    }

    private IEnumerator SpawnProcess()
    {
        bool isCreateComplete = true;
        while (true)
        {
            if (isCreateComplete)
            {
                // 정상적인 재생성 간격
                yield return YieldInstructionCache.WaitForSeconds(createInterval);
            }
            else
            {
                // 생성 실패하는 경우
                yield return YieldInstructionCache.WaitForSeconds(1f);  // 혹시 모를 무한 반복 루프 및 과부하를 위해 1초만 대기한다.
            }

            // 좀비 생성하기
            CreateZombie();
        }

        void CreateZombie()
        {
            Vector3 direction = Random.insideUnitSphere;
            direction.y = 0f;
            direction = direction.normalized;
            Vector3 createPos = _target.MyTransform.position + direction * createDistance;
            if (NavMesh.SamplePosition(createPos, out var hit, 10f, NavMesh.AllAreas))
            {
                createPos = hit.position;
                var zombie = GameResourceManager.Instance.CreateCharacter<Zombie>(eCharacterType.Zombie, createPos);
                zombie.gameObject.SetActive(true);
                zombie.Initiailize();
                NavMeshPath path = new NavMeshPath();
                zombie.Agent.CalculatePath(GamePlayManager.Instance.player.MyTransform.position, path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    isCreateComplete = true;
                }
                else
                {
                    // 엉뚱한 위치에 생성되면 로직 재실행 유도
                    zombie.gameObject.SetActive(false);
                    isCreateComplete = false;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_target == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_target.MyTransform.position, createDistance);
    }
}