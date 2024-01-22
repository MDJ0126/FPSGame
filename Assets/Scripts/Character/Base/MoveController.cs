using UnityEngine;

namespace Model.Character
{
    [RequireComponent(typeof(Character))]
    public class MoveController : MonoBehaviour
    {
        private Character _character;
        private Rigidbody Rigidbody => _character.Rigidbody;
        private float _moveSpeed = 5f;
        private bool _isMouseLock = true;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void FixedUpdate()
        {
            UpdateMove();
            UpdateRotation();
        }

        private void Update()
        {
#if UNITY_EDITOR
            SetActiveMouseLock();
#endif
        }

        private void UpdateMove()
        {
            Vector3 dir = Vector3.zero;
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            Rigidbody.MovePosition(this.transform.position + dir * Time.deltaTime * _moveSpeed);
        }

        private void UpdateRotation()
        {
            Vector3 dir = Vector3.zero;
            dir.x = Input.GetAxis("Mouse X");
            dir.z = Input.GetAxis("Mouse Y");
            //Rigidbody.MoveRotation(this.transform.rotation * Quaternion.Euler(dir));

            //_character.MyTransform.forward = dir;
        }

        private void SetActiveMouseLock()
        {
            if (Input.GetKeyUp(KeyCode.L))
            {
                _isMouseLock = !_isMouseLock;
            }

            if (_isMouseLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}