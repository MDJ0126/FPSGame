using Cinemachine;
using UnityEngine;

namespace FPSGame.Character
{
    /// <summary>
    /// 플레이어 컨트롤러 (나 자신)
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

            //Debug.DrawRay(_owner.aimCenter.position, _owner.MyTransform.forward * 100f, Color.red);
            //Debug.DrawRay(_camTransform.position, _owner.MyTransform.forward * 100f, Color.red);
        }

        /// <summary>
        /// 마우스 입력 업데이트
        /// </summary>
        private void UpdateMouse()
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            //_moveController.AddRotate(new Vector3(0f, x, 0f));
            _owner.UpdateAimRotation(new Vector3(x, y, 0f));

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
        /// 이동 입력 업데이트
        /// </summary>
        private void UpdateMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (horizontal != 0f || vertical != 0f)
            {
                // 타겟 방향 계산
                Vector3 targetDirection = _owner.aim.position - _camTransform.position;

                // 타겟 방향의 Y축 회전값만 계산
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                var v = _owner.aimCenter.rotation;

                // Y축 회전값만을 적용하여 캐릭터 회전
                transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

                // 에이밍센터 Y축 초기화
                _owner.aimCenter.rotation = v;

                // 회전이 완료된 캐릭터 기준으로 이동 처리
                _moveController.Move(_owner.MyTransform.right * horizontal + _owner.MyTransform.forward * vertical);
            }
        }
    }
}