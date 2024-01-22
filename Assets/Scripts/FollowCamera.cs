using UnityEngine;

namespace Model.Character
{
    [ExecuteInEditMode]
    public class FollowCamera : MonoBehaviour
    {
        #region Inspector

        public Camera camera;
        public Transform target;
        public Vector3 offset = new Vector3(1.7f, 1.4f, -2.7f);

        #endregion

        private Transform _transform;
        public Transform MyTransform
        {
            get
            {
                if (!_transform) _transform = this.transform;
                return _transform;
            }
        }

        private Transform _cameraTransform;
        public Transform CameraTransform
        {
            get
            {
                if (!_cameraTransform) _cameraTransform = camera.transform;
                return _cameraTransform;
            }
        }

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
                this.MyTransform.position = desiredPosition;
                this.MyTransform.rotation = target.rotation;
            }
        }
    }
}