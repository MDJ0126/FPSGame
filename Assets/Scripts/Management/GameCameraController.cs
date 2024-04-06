using Cinemachine;
using System.Collections;
using UnityEngine;

public class GameCameraController : SingletonBehaviour<GameCameraController>
{
	#region Inspector

	public Camera baseCamera;
	public CinemachineVirtualCamera virtualCamera;

	[Header("Variables")]
	public float zoomValue = -0.25f;

    #endregion

    private Cinemachine3rdPersonFollow _thirdperson;
    private float _currentCameraDistance = 0f;
	private Coroutine _zoomCoroutine = null;

    private void Awake()
    {
        _thirdperson = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
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
                _currentCameraDistance = Mathf.Lerp(_thirdperson.CameraDistance, 0, time);
            }
            _thirdperson.CameraDistance = _currentCameraDistance;
            yield return null;
		}
		_thirdperson.CameraDistance = _currentCameraDistance;
    }
}