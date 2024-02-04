using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Model.Character
{
    [ExecuteInEditMode]
    public class FollowCamera : MonoBehaviour
    {
        #region Inspector

        public Camera camera;
        public Transform target;
        public Vector3 offset = new Vector3(1.7f, 1.4f, -2.7f);
        public float rotateSpeed = 150f;

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
            //UpdateRotate();
        }

        private void OnValidate()
        {
            Follow();
        }

        private void UpdateRotate()
        {
            Vector3 dir = Vector3.zero;
            dir.x = Input.GetAxis("Mouse Y");
            if (dir != Vector3.zero)
            {
                dir = dir.normalized;
                CameraTransform.eulerAngles -= dir * rotateSpeed * Time.deltaTime;
            }
        }

        private void Follow()
        {
            if (target != null)
            {
                Vector3 desiredPosition = target.position + target.TransformDirection(offset);
                this.MyTransform.position = Vector3.Lerp(this.MyTransform.position, desiredPosition, 0.9f);
                this.MyTransform.rotation = Quaternion.Lerp(this.MyTransform.rotation, target.rotation, 0.9f);
            }
        }
    }
}