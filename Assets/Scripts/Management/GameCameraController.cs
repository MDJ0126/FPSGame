using Cinemachine;
using FPSGame.Character;
using System;
using System.Collections;
using UnityEngine;

public class GameCameraController : SingletonBehaviour<GameCameraController>
{
    private const float AIM_DISTANCE = 10f;

    #region Inspector

	public PlayerCharacter player;
    public Camera baseCamera;
	public CinemachineVirtualCamera virtualCamera;
	public Transform aimRay;

	[Header("Variables")]
	public float zoomValue = -0.25f;

    #endregion

    private Cinemachine3rdPersonFollow _thirdperson;
	private float _prevCameraDistance = 0f;
    private float _currentCameraDistance = 0f;
	private Coroutine _zoomCoroutine = null;
	private CursorLockMode _lockMode = CursorLockMode.Locked;
	private Vector3 _rotate = Vector3.zero;

    private void Awake()
    {
        _thirdperson = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
		_prevCameraDistance = _thirdperson.CameraDistance;
    }

    private void Update()
    {
        UpdateCursorLock();

        Vector3 rayOrigin = baseCamera.transform.position;
        Quaternion rotation = Quaternion.Euler(-_rotate.y, _rotate.x, 0f);
        Vector3 rayDirection = rotation * player.transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * AIM_DISTANCE, Color.red);
		RaycastHit[] hitInfos = Physics.RaycastAll(rayOrigin, rayDirection, AIM_DISTANCE);
		RaycastHit firstHit = Array.Find(hitInfos, info => !info.collider.gameObject.Equals(player.gameObject));
        if (firstHit.collider != null)
		{
			aimRay.position = firstHit.point;
        }
		else
		{
			aimRay.position = baseCamera.transform.position + rayDirection * AIM_DISTANCE;
        }

		_rotate = Vector3.zero;
    }

    private void UpdateCursorLock()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_lockMode = CursorLockMode.None;
		}
		else
		{
			_lockMode = CursorLockMode.Locked;
		}
		Cursor.lockState = _lockMode;
    }

    public void UpdateRotation(float x, float y)
    {
		_rotate.x = x;
		_rotate.y = y;
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
		float time =  (1f - (zoomValue - _currentCameraDistance) / zoomValue) * ZOOM_SPEED;
		while (time < ZOOM_SPEED)
		{
			time += Time.deltaTime;
			if (isZoomIn)
			{
				_currentCameraDistance = Mathf.Lerp(_thirdperson.CameraDistance, zoomValue, time);
			}
			else
			{
                _currentCameraDistance = Mathf.Lerp(_thirdperson.CameraDistance, _prevCameraDistance, time);
            }
            _thirdperson.CameraDistance = _currentCameraDistance;
            yield return null;
		}
		_thirdperson.CameraDistance = _currentCameraDistance;
    }
}