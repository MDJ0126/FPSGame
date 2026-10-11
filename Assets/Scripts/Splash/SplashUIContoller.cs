using System.Collections;
using UnityEngine;

public class SplashUIContoller : MonoBehaviour
{
    #region Inspector

    public CanvasGroup canvasGroup;
    public float animationDuration = 1f;
    public float exposureTime = 1f;

    #endregion

    private float _time = 0f;

    private IEnumerator Start()
    {
        yield return SplashAnimation();
        SceneController.Instance.LoadScene(eScene.Lobby);
    }

    /// <summary>
    /// 스플래시 애니메이션
    /// </summary>
    /// <returns></returns>
    private IEnumerator SplashAnimation()
    {
        _time = 0f;
        canvasGroup.alpha = 0f;
        var easingFunc = EasingFunction.GetEasingFunction(eEaseType.Linear);
        while (_time < 1f)
        {
            _time += Time.deltaTime;
            var value = easingFunc(0f, 1f, _time);
            canvasGroup.alpha = value;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        yield return YieldInstructionCache.WaitForSeconds(exposureTime);
    }
}