using System.Collections;
using UnityEngine;

public class FadeController : SingletonBehaviour<FadeController>
{
    [RuntimeInitializeOnLoadMethod]
    private static void Load()
    {
        if (!IsLive)
        {
            if (Application.isPlaying)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/FadeController"));
            }
        }
    }

    #region Inspector

    public CanvasGroup canvasGroup;

	#endregion

	public bool IsFade { get; private set; } = false;
	private float FillAmount => canvasGroup.alpha;
    private Coroutine _coroutine = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

	private void Initialize()
    {
        this.IsFade = false;
        canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 페이드인
    /// </summary>
    /// <param name="duration"></param>
    public void FadeIn(float duration = 0.5f)
	{
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(FadeProcess(true, duration));

    }

    /// <summary>
    /// 페이드아웃
    /// </summary>
    /// <param name="duration"></param>
	public void FadeOut(float duration = 0.5f)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(FadeProcess(false, duration));
    }

	private IEnumerator FadeProcess(bool isFade, float duration)
	{
		var easeFunction = EasingFunction.GetEasingFunction(eEaseType.Linear);
		if (isFade)
        {
            float time = this.FillAmount;
            float start = 0f;
            float end = 1f;
            while (this.canvasGroup.alpha < end)
            {
                time += Time.deltaTime;
                this.canvasGroup.alpha = easeFunction(start, end, time / duration);
                yield return null;
            }
            this.canvasGroup.alpha = end;
        }
		else
        {
            float time = 1f - this.FillAmount;
            float start = 1f;
			float end = 0f;
            while (this.canvasGroup.alpha > end)
			{
				time += Time.deltaTime;
				this.canvasGroup.alpha = easeFunction(start, end, time / duration);
				yield return null;
			}
			this.canvasGroup.alpha = end;
        }
    }
}