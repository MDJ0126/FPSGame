using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using FPSGame.Character;

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

	private float _prevCameraFOV = 0f;
    private float _currentCameraFOV = 0f;
	private Coroutine _zoomCoroutine = null;
	private float _rotateY = 0f;

    private void Awake()
    {
		_prevCameraFOV = virtualCamera.m_Lens.FieldOfView;
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

    public void UpdateRotationY(float y)
    {
		_rotateY = y;
    }

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