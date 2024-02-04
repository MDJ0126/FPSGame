using UnityEngine;

public class CharacterHipController : MonoBehaviour
{
	#region Inspector

	public Transform hipRoot;
	public CharacterFootIK leftFootIK;
	public CharacterFootIK rightFootIK;

    #endregion

    private bool _isMoveing = false;
    private bool _isIdle => !_isMoveing;

    private Vector3 _center = Vector3.zero;
    private Vector3 _currentCenter = Vector3.zero;

    private void Start()
    {
        UpdateCenter();
    }

    private void Update()
    {
        Vector3 start = leftFootIK.TwoBoneIKConstraint.data.target.position;
        Vector3 dir = (rightFootIK.TwoBoneIKConstraint.data.target.position - leftFootIK.TwoBoneIKConstraint.data.target.position).normalized;
        float distance = Vector3.Distance(leftFootIK.TwoBoneIKConstraint.data.target.position, rightFootIK.TwoBoneIKConstraint.data.target.position);
        Debug.DrawRay(start, dir * distance, Color.green);
    }

    private void UpdateCenter()
    {
        Vector3 left = leftFootIK.NewPosition;
        left.y = 0f;
        Vector3 right = rightFootIK.NewPosition;
        right.y = 0f;
        _center = Vector3.Lerp(left, right, 0.5f);
        _center.y = 0f;
        _currentCenter = this.transform.position;
        _currentCenter.y = _center.y;
    }

    private void FixedUpdate()
    {
        UpdateCenter();
        if (Vector3.Distance(_center, _currentCenter) > 0.3f)
        {
            Vector3 dir = _currentCenter - _center;
            float centerToLeftDistance = Vector3.Distance(_currentCenter, leftFootIK.NewPosition);
            float centerToRightDistance = Vector3.Distance(_currentCenter, rightFootIK.NewPosition);

            if (centerToLeftDistance < centerToRightDistance)
            {
                rightFootIK.Move(rightFootIK.forward.position);
            }
            else
            {
                leftFootIK.Move(leftFootIK.forward.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(leftFootIK.TwoBoneIKConstraint.data.target.position, 0.2f);
        Gizmos.DrawWireSphere(rightFootIK.TwoBoneIKConstraint.data.target.position, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_center, 0.05f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_currentCenter, 0.05f);
    }
}
