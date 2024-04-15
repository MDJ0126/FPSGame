using UnityEngine;

namespace FPSGame.Character
{
    /// <summary>
    /// 플레이어 컨트롤러 (나 자신)
    /// </summary>
    [RequireComponent(typeof(PlayerCharacter), typeof(MoveController))]
    public class PlayerInputController : MonoBehaviour
    {
        private GameCameraController _cameraController;
        private PlayerCharacter _owner = null;
        private MoveController _moveController = null;
        private CursorLockMode _lockMode = CursorLockMode.Locked;

        private void Awake()
        {
            _cameraController = GameCameraController.Instance;
            _owner = GetComponent<PlayerCharacter>();
            _moveController = GetComponent<MoveController>();
        }

        private void Update()
        {
            _owner.aim.position = _cameraController.aimRay.position;
            UpdateCursorLock();
            UpdateMouse();
            UpdateMovement();
            UpdateSummon();
        }

        /// <summary>
        /// 커서 잠금
        /// </summary>
        private void UpdateCursorLock()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _lockMode = CursorLockMode.None;
            }
            else
            {
                _lockMode = CursorLockMode.Locked;
            }
            Cursor.lockState = _lockMode;
        }

        /// <summary>
        /// 소환 체크
        /// </summary>
        private void UpdateSummon()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Vector3 randomVector = Random.insideUnitSphere;
                var pos = _owner.MyTransform.position + new Vector3(randomVector.x, 0f, randomVector.z);
                var bot = GameResourceManager.Instance.CreateCharacter<BotCharacter>(eCharacterType.Human, pos);
                bot.gameObject.SetActive(true);
                bot.Initiailize();
            }
        }

        /// <summary>
        /// 마우스 입력 업데이트
        /// </summary>
        private void UpdateMouse()
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            if (x != 0 || y != 0)
            {
                _cameraController.UpdateRotationY(y);
                _moveController.UpdateRotation(new Vector3(x, 0f, 0f));
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
        /// 이동 입력 업데이트
        /// </summary>
        private void UpdateMovement()
        {
            // 이동
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            //if (horizontal != 0f || vertical != 0f)
            {
                _moveController.Move(GameCameraController.Instance.baseCamera.transform.right * horizontal + GameCameraController.Instance.baseCamera.transform.forward * vertical);
            }

            // 점프
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _moveController.Jump();
            }
        }
    }
}