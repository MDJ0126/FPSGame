using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonBehaviour<SceneController>
{
    private Coroutine _coroutine = null;

    /// <summary>
    /// 씬 로드하기
    /// </summary>
    /// <param name="scene"></param>
    public void LoadScene(eScene scene)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(LoadSceneAsyncReleaseMemory(scene));
    }

    private IEnumerator LoadSceneAsyncReleaseMemory(eScene scene)
    {
        // 페이드인
        float duration = 0.5f;
        FadeController.Instance.FadeIn(duration);
        yield return YieldInstructionCache.WaitForSeconds(duration);

        // 빈 씬 로드로 이전 씬의 메모리를 완전 초기화
        //GC.Collect();
        //Resources.UnloadUnusedAssets();
        yield return LoadSceneProcess(eScene.Empty);

        // 새로운 씬 로드
        yield return LoadSceneProcess(scene);

        // 페이드아웃
        FadeController.Instance.FadeOut();
    }

    private IEnumerator LoadSceneProcess(eScene scene)
    {
        var async = SceneManager.LoadSceneAsync((int)scene);
        while (!async.isDone)
        {
            yield return null;
            float progress = async.progress * 100f;
            if (progress >= 0.9f)
            {
                if (scene != eScene.Empty)
                    yield return YieldInstructionCache.WaitForSeconds(1f);
                async.allowSceneActivation = true;
                yield return null;
            }
        }
    }
}