using UnityEngine;

public abstract class FollowHUD : MonoBehaviour
{
    #region Inspector

    public GameObject baseGroup;

    #endregion

    private Transform _transform = null;
    public Transform MyTransform
    {
        get
        {
            if (_transform == null)
            {
                _transform = this.transform;
            }
            return _transform;
        }
    }

    [HideInInspector] public Transform target;
    private Camera _targetCamera = null;

    private float _height = 0f;

    public void SetTarget(Transform target)
    {
        if (target != null)
        {
            this.target = target;
            _targetCamera = Utils.GetMyCamera(target.gameObject);
            Update();
        }
    }

    public void SetHeight(float height)
    {
        _height = height;
    }

    private void OnDisable()
    {
        this.target = null;
        _targetCamera = null;
    }

    private void Update()
    {
        if (target == null || _targetCamera == null)
            return;

        // 타겟의 스크린 좌표를 가져온다.
        Vector3 viewPort = _targetCamera.WorldToViewportPoint(target.position);
        Vector3 targetPos = _targetCamera.ViewportToScreenPoint(viewPort);
        
        // 카메라 안에 오브젝트가 있는지 확인한다.
        bool inArea = viewPort.z > 0f && viewPort.x > 0f && viewPort.x < 1f && viewPort.y > 0f && viewPort.y < 1f;
        if (inArea)
        {
            // UI의 위치를 변경해준다.
            this.MyTransform.position = new Vector3(targetPos.x, targetPos.y + _height, targetPos.z);
            baseGroup.SetActive(true);
        }
        else
        {
            // 타겟이 카메라 뒤에 있으면 UI를 비활성화한다.
            baseGroup.SetActive(false);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}