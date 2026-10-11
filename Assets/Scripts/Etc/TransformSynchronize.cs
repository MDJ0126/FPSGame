using System;
using UnityEngine;

public class TransformSynchronize : MonoBehaviour
{
    [Flags]
    public enum eUpdateType : byte
    {
        None = 0,
        Position = 1 << 0,
        Rotation = 1 << 1,
        Everything = byte.MaxValue,
    }

    private Transform _myTransform = null;
    public Transform MyTransform
    {
        get
        {
            if (_myTransform == null)
                _myTransform = transform;
            return _myTransform;
        }
    }

    public Transform target;
    public eUpdateType updateType = eUpdateType.Everything;

    private void OnValidate()
    {
        UpdateSynchronize();
    }

    private void LateUpdate()
    {
        UpdateSynchronize();
    }

    private void UpdateSynchronize()
    {
        if (target == null) return;
        if ((updateType & eUpdateType.Position) != 0) this.MyTransform.position = target.position;
        if ((updateType & eUpdateType.Rotation) != 0) this.MyTransform.rotation = target.rotation;
    }
}
