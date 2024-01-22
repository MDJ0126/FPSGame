using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Character
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {
        private Transform _transform;
        public Transform MyTransform
        {
            get
            {
                if (!_transform) _transform = GetComponent<Transform>();
                return _transform;
            }
        }

        #region Inspector

        public Transform target;
        public float smoothSpeed = 0.125f;
        public float lookSpeed = 2f;
        public Vector3 offset = new Vector3(-0.7f, 1.4f, -2.7f);

        #endregion

        private void FixedUpdate()
        {
            Follow();
        }

        private void OnValidate()
        {
            Follow();
        }

        private void Follow()
        {
            if (target != null)
            {
                Vector3 desiredPosition = target.position + target.TransformDirection(offset);
                MyTransform.position = desiredPosition;
                MyTransform.rotation = target.rotation;
            }
        }

        private void RotateCamera()
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            MyTransform.Rotate(0f, mouseX, 0f);
        }
    }
}