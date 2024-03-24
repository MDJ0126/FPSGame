using Cinemachine;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace FPSGame.Character
{
    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ� (�� �ڽ�)
    /// </summary>
    [RequireComponent(typeof(Character), typeof(MoveController))]
    public class PlayerInputController : MonoBehaviour
    {
        private Character _owner = null;
        private MoveController _moveController = null;
        private CinemachineVirtualCamera _cam = null;
        private Transform _camTransform = null;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _moveController = GetComponent<MoveController>();
            _cam = GameObject.Find("Player Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            _camTransform = _cam.transform;
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
            //_moveController.AddRotate(new Vector3(0f, x, 0f));
            _owner.UpdateAimRotation(new Vector3(x, y, 0f));
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
                // Ÿ�� ���� ���
                Vector3 targetDirection = _owner.aim.position - transform.position;

                // Ÿ�� ������ Y�� ȸ������ ���
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                // Y�� ȸ�������� �����Ͽ� ĳ���� ȸ��
                transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

                // ���ּ̹��� Y�� �ʱ�ȭ
                _owner.aimCenter.localEulerAngles = new Vector3(_owner.aimCenter.localEulerAngles.x, 0f, _owner.aimCenter.localEulerAngles.z);

                // ȸ���� �Ϸ�� ĳ���� �������� �̵� ó��
                _moveController.Move(_owner.MyTransform.right * horizontal + _owner.MyTransform.forward * vertical);
            }
        }
    }
}