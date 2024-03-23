using UnityEngine;

namespace FPSGame.Character
{
    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ� (�� �ڽ�)
    /// </summary>
    [RequireComponent(typeof(Character), typeof(MoveController))]
    public class PlayerInput : MonoBehaviour
    {
        private Character _owner = null;
        private MoveController _moveController = null;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _moveController = GetComponent<MoveController>();
        }

        private void LateUpdate()
        {
            UpdateMouse();
            UpdateMovement();
        }

        /// <summary>
        /// ���콺 �Է� ������Ʈ
        /// </summary>
        private void UpdateMouse()
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            //_moveController.AddRotate(new Vector3(-y, x, 0f));
            _moveController.AddRotate(new Vector3(0f, x, 0f));
        }

        /// <summary>
        /// �̵� �Է� ������Ʈ
        /// </summary>
        private void UpdateMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _moveController.Move(_owner.MyTransform.right * horizontal + _owner.MyTransform.forward * vertical);
        }
    }
}