using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(TwoBoneIKConstraint))]
public class CharacterFootIK : MonoBehaviour
{
    private const float MOTION_SPEED = 4f;
    private const float STEP_HEIGHT = 0.5f;

    #region Inspector

    public Transform forward;
    public Transform back;

	#endregion

	public TwoBoneIKConstraint TwoBoneIKConstraint { get; private set; } = null;
    private Vector3 _oldPosition = Vector3.zero;
    public Vector3 NewPosition { get; private set; } = Vector3.zero;
    private Vector3 _currentPos = Vector3.zero;
    private float _lerp = 1f;

    private void Awake()
    {
        TwoBoneIKConstraint = GetComponent<TwoBoneIKConstraint>();
        _oldPosition = NewPosition = _currentPos = TwoBoneIKConstraint.data.target.position;
    }

    private void FixedUpdate()
    {
        if (_lerp < 1f)
        {
            _lerp += Time.deltaTime * MOTION_SPEED;
            _lerp = Mathf.Min(_lerp, 1f);
            _currentPos = Vector3.Lerp(_oldPosition, NewPosition, _lerp);
            _currentPos.y = Mathf.Sin(Mathf.PI * _lerp) * STEP_HEIGHT;
        }
        TwoBoneIKConstraint.data.target.position = _currentPos;
    }

    public void Move(Vector3 wolrdPosition)
    {
        _lerp = 0f;
        _oldPosition = NewPosition;
        NewPosition = wolrdPosition;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_oldPosition, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(NewPosition, 0.2f);
    }
}
