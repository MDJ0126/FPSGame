using UnityEngine;

namespace FPSGame.Character
{
    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ� (�� �ڽ�)
    /// </summary>
    [RequireComponent(typeof(PlayerCharacter), typeof(MoveController))]
    public class PlayerInputController : MonoBehaviour
    {
        private GameCameraController _cameraController;
        private PlayerCharacter _owner = null;
        private MoveController _moveController = null;

        private void Awake()
        {
            _cameraController = GameCameraController.Instance;
            _owner = GetComponent<PlayerCharacter>();
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

            if (x != 0 || y != 0)
            {
                _cameraController.UpdateAimRotation(new Vector3(x, y, 0f));
                _owner.aim.position = _cameraController.aimRay.position;
            }

            // Left Click
            if (Input.GetMouseButton(0))
            {
                _owner.WeaponHandler.Fire();
            }

            // Right Click
            if (Input.GetMouseButton(1))
            {
                GameCameraController.Instance.Zoom(true);
            }
            else
            {
                GameCameraController.Instance.Zoom(false);
            }
        }

        /// <summary>
        /// �̵� �Է� ������Ʈ
        /// </summary>
        private void UpdateMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (horizontal != 0f || vertical != 0f)
            {
                _owner.MyTransform.LookAt(_cameraController.aimRay);

                // ȸ���� �Ϸ�� ĳ���� �������� �̵� ó��
                _moveController.Move(_owner.MyTransform.right * horizontal + _owner.MyTransform.forward * vertical);
            }
        }
    }
}