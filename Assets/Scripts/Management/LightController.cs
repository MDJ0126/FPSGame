using System.Collections;
using UnityEngine;

public class LightController : SingletonBehaviour<LightController>
{
    #region Inspector

    public Light lightObject;
    public Color onColor;
    public Color offColor;
    public float loopTime = 10f;

    #endregion

    private Coroutine _coroutine = null;

    private void Start()
    {
        StartCoroutine("LoopCo");
    }

    private IEnumerator LoopCo()
    {
        bool isOn = true;
        while (true)
        {
            if (isOn)
            {
                isOn = false;
            }
            else
            {
                isOn = true;
            }
            SetLightState(isOn);
            yield return YieldInstructionCache.WaitForSeconds(loopTime);
        }
    }

    public void SetLightState(bool isOn, float duration = 5f)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ChangeStateProcess(isOn, duration));
    }

    private IEnumerator ChangeStateProcess(bool isOn, float duration)
    {
        float factor = 0f;
        Color start, end;
        start = lightObject.color;
        end = isOn ? onColor : offColor;
        while (factor < 1f)
        {
            factor += Time.deltaTime / duration;
            lightObject.color = Color.Lerp(start, end, factor);
            yield return null;
        }
        lightObject.color = end;
    }
}