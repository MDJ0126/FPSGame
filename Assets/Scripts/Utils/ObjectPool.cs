using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // 30초마다 오래 사용하지 않은 오브젝트를 삭제한다.
    private const float REFRESH_TIME_PER_SECONDS = 30f;

    private class PoolItem
    {
        public GameObject gameObject;

        public bool isActive => gameObject.activeSelf;

        public DateTime lastActiveTime = DateTime.Now;
    }

    #region Inspector

    public GameObject original;
    public int count = 10;

    #endregion;

    private List<PoolItem> _pool = new();

    private DateTime _lastRefreshedTime = DateTime.Now;

    private string _originalName = string.Empty;

    private bool _isInit = false;

    public int ActiveCount => _pool.FindAll(o => o.gameObject.activeSelf)?.Count ?? 0;

    private void SetName(string ObjectName)
    {
        _originalName = ObjectName;
    }

    private void Start()
    {
        //original.SetActive(false);
        Initialize();
    }

    private void Update()
    {
        UpdateName();
    }

    private void Initialize()
    {
        if (!_isInit)
        {
            _isInit = true;
            SetName(this.gameObject.name);
            int tempCount = count;
            count = 0;
            for (int i = 0; i < tempCount; i++)
            {
                Create();
            }
        }
    }

    private PoolItem Create()
    {
        GameObject go = Instantiate(original, this.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        go.SetActive(false);
        PoolItem item = new PoolItem { gameObject = go };
        _pool.Add(item);
        count++;
        UpdateName();
        return item;
    }

    public GameObject Get()
    {
        if (!_isInit) Initialize();
        var item = _pool.Find(poolItem => !poolItem.gameObject.activeSelf);
        if (item == null) item = Create();
        item.lastActiveTime = DateTime.Now;
        AutoReleaseMemory();
        return item.gameObject;
    }

    public T Get<T>() where T : Component
    {
        if (!_isInit) Initialize();
        var item = _pool.Find(poolItem => !poolItem.gameObject.activeSelf);
        if (item == null) item = Create();
        item.lastActiveTime = DateTime.Now;
        AutoReleaseMemory();
        return item.gameObject.GetComponent<T>();
    }

    private void AutoReleaseMemory()
    {
        var nowTime = DateTime.Now;
        if (_lastRefreshedTime.AddSeconds(REFRESH_TIME_PER_SECONDS) < nowTime)
        {
            _lastRefreshedTime = nowTime;
            for (int i = _pool.Count - 1; i >= 0; --i)
            {
                var item = _pool[i];
                if (!item.gameObject.activeSelf && item.lastActiveTime.AddSeconds(REFRESH_TIME_PER_SECONDS) < nowTime)
                {
                    _pool.RemoveAt(i);
                    Destroy(item.gameObject);
                    count--;
                }
            }
        }
        UpdateName();
    }

    private void UpdateName()
    {
#if UNITY_EDITOR
        this.gameObject.name = $"{_originalName} ({this.ActiveCount}/{count})";
#endif
    }
}