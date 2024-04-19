
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Transform 초기화 (확장 함수)
    /// </summary>
    /// <param name="transform"></param>
    public static void Initialize(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 오브젝트를 비추고 있는 카메라 가져오기
    /// </summary>
    /// <returns></returns>
    public static Camera GetMyCamera(GameObject gameObject)
    {
        foreach (Camera camera in Camera.allCameras)
        {
            var cullingMask = 1 << gameObject.layer;
            if ((camera.cullingMask & cullingMask) != 0)
                return camera;
        }
        return null;
    }
}