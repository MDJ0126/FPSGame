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

        private void LateUpdate()
        {
            Follow();
        }

        private void Follow()
        {
            if (target != null)
            {
                // Calculate the desired position based on the target's position and the offset
                Vector3 desiredPosition = target.position + target.TransformDirection(offset);

                // Set the camera's position to the desired position
                MyTransform.position = desiredPosition;

                // Rotate the camera to look at the target
                MyTransform.rotation = target.rotation;
            }
        }

        private void RotateCamera()
        {
            // Get the mouse input for camera rotation
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;

            // Rotate the character (not the camera) based on the mouse input for horizontal look
            MyTransform.Rotate(0f, mouseX, 0f);
        }
    }
}