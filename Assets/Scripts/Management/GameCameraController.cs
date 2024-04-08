using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class GameCameraController : SingletonBehaviour<GameCameraController>
{
	#region Inspector

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

    private void Awake()
    {
        _thirdperson = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
		_prevCameraDistance = _thirdperson.CameraDistance;
    }

    private void Update()
    {
		UpdateCursorLock();

        const float DISTANCE = 10f;
        Debug.DrawRay(virtualCamera.transform.position, virtualCamera.transform.forward * DISTANCE, Color.red);
		RaycastHit[] hitInfos = Physics.RaycastAll(virtualCamera.transform.position, virtualCamera.transform.forward, DISTANCE);
		RaycastHit firstHit = Array.Find(hitInfos, info => !info.collider.CompareTag("Player"));
        if (firstHit.collider != null)
		{
			aimRay.position = firstHit.point;
        }
		else
		{
			aimRay.position = virtualCamera.transform.position + virtualCamera.transform.forward * DISTANCE;
        }
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

    public void UpdateAimRotation(Vector3 rotate)
    {
        Vector3 angle = virtualCamera.transform.eulerAngles;
        virtualCamera.transform.rotation = Quaternion.Euler(angle.x - rotate.y, angle.y + rotate.x, angle.z);
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