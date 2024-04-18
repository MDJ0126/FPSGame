using System;
using System.Collections;
using Cinemachine;
using FPSGame.Character;
using UnityEngine;

public class GameCameraController : SingletonBehaviour<GameCameraController>
{
    private const float AIM_DISTANCE = 15f;

    #region Inspector

    public PlayerCharacter player;
    public Camera baseCamera;
    public CinemachineVirtualCamera virtualCamera;
    public Transform aimRay;

    [Header("Variables")]
    public float zoomFOV = 40f;

    #endregion

    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin = null;
    private float _prevCameraFOV = 0f;
    private float _currentCameraFOV = 0f;
    private Coroutine _zoomCoroutine = null;
    private float _rotateY = 0f;
    private Coroutine _shakeCoroutine = null;

    private void Awake()
    {
        _prevCameraFOV = virtualCamera.m_Lens.FieldOfView;
        _cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        var followTarget = virtualCamera.Follow;

        var rotate = followTarget.eulerAngles + new Vector3(-_rotateY, 0f, 0f);
        Quaternion newRotation = Quaternion.Euler(rotate);
        followTarget.rotation = newRotation;

        Vector3 rayOrigin = followTarget.position;
        Vector3 rayDirection = followTarget.forward;
        Debug.DrawRay(rayOrigin, rayDirection * AIM_DISTANCE, Color.red);

        LayerMask ignoreLayer = 1 << (int)eLayer.IgnoreRaycast;
        RaycastHit[] hitInfos = Physics.RaycastAll(rayOrigin, rayDirection, AIM_DISTANCE, layerMask: ~ignoreLayer);
        RaycastHit firstHit = Array.Find(hitInfos, info => !info.collider.gameObject.Equals(player.gameObject));
        if (firstHit.collider != null)
        {
            aimRay.position = firstHit.point;
        }
        else
        {
            aimRay.position = followTarget.position + rayDirection * AIM_DISTANCE;
        }

        _rotateY = 0f;
    }

    /// <summary>
    /// 카메라 쉐이크
    /// </summary>
    /// <param name="easeType">이징 타입</param>
    /// <param name="duration">재생 시간</param>
    public void Shake(eEaseType easeType, float gain, float duration = 0.5f, float delay = 0.0f)
    {
        if (_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
        _shakeCoroutine = StartCoroutine(ShakeProcess(easeType, gain, duration, delay));
    }

    private IEnumerator ShakeProcess(eEaseType easeType, float gain, float duration = 0.5f, float delay = 0.0f)
    {
        if (delay != 0)
        {
            yield return new WaitForSeconds(delay);
        }

        float playTime = 0f;
        while (playTime <= duration)
        {
            playTime += Time.deltaTime;
            float t = playTime / duration;
            var tweenFuntion = EasingFunction.GetEasingFunctionDerivative(easeType);
            float value = tweenFuntion(0f, gain, t);
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(value, 0f, t);
            _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(value, 0f, t);
            yield return null;
        }
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;
    }

    /// <summary>
    /// 카메라 Y축 회전
    /// </summary>
    /// <param name="y"></param>
    public void UpdateRotationY(float y)
    {
        _rotateY = y;
    }

    /// <summary>
    /// 카메라 줌
    /// </summary>
    /// <param name="isZoomIn"></param>
    public void Zoom(bool isZoomIn)
    {
        if (_zoomCoroutine != null)
            StopCoroutine(_zoomCoroutine);
        _zoomCoroutine = StartCoroutine(ZoomAnimation(isZoomIn));
    }

    private IEnumerator ZoomAnimation(bool isZoomIn)
    {
        const float ZOOM_SPEED = 0.1f;
        float time = ((_prevCameraFOV - _currentCameraFOV) / zoomFOV) * ZOOM_SPEED;
        while (time < ZOOM_SPEED)
        {
            time += Time.deltaTime;
            if (isZoomIn)
            {
                _currentCameraFOV = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, zoomFOV, time);
            }
            else
            {
                _currentCameraFOV = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, _prevCameraFOV, time);
            }
            virtualCamera.m_Lens.FieldOfView = _currentCameraFOV;
            yield return null;
        }

        if (isZoomIn)
        {
            _currentCameraFOV = zoomFOV;
        }
        else
        {
            _currentCameraFOV = _prevCameraFOV;
        }
        virtualCamera.m_Lens.FieldOfView = _currentCameraFOV;
    }
}