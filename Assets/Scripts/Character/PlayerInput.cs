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
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _moveController.Move(_owner.MyTransform.right * horizontal + _owner.MyTransform.forward * vertical);
        }
    }
}