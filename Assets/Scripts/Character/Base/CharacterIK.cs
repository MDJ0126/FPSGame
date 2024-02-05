using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterIK : MonoBehaviour
{
    public float heightOffset = 0.2f;

    private Animator _animator = null;
    private Vector3 _raypointLeft = Vector3.zero;
    private Vector3 _raypointRight = Vector3.zero;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        float groundDistance = 0f;

        // 왼쪽 발 다리 조절
        _raypointLeft = UpdateFoot(HumanBodyBones.LeftFoot, AvatarIKGoal.LeftFoot);

        // 오른쪽 발 다리 조절
        _raypointRight = UpdateFoot(HumanBodyBones.RightFoot, AvatarIKGoal.RightFoot);

        // 경사로에서 다리를 뻗은 만큼 전체적으로 몸체를 내려준다.
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.up * -groundDistance, 0.5f);

        /// 발 다리 조절 함수
        Vector3 UpdateFoot(HumanBodyBones bodyBones, AvatarIKGoal avatarIKGoal)
        {
            Transform footTransform = _animator.GetBoneTransform(bodyBones);
            Vector3 dir = Vector3.down;
            float distance = 2f;

            // 레이케스팅하여 레이 충돌 지점 + 오프셋 위치 만큼 발을 고정한다.
            Debug.DrawRay(footTransform.position + Vector3.up, dir * distance);
            if (Physics.Raycast(footTransform.position + Vector3.up, dir , out RaycastHit hit, distance))
            {
                float diff = Vector3.Distance(hit.point, footTransform.position);
                _animator.SetIKPosition(avatarIKGoal, hit.point + hit.normal * heightOffset);
                _animator.SetIKPositionWeight(avatarIKGoal, 1f);
                Debug.DrawRay(hit.point, hit.normal);
                if (Physics.Raycast(footTransform.position, dir, out RaycastHit hit2))
                {
                    _animator.SetIKRotation(avatarIKGoal, Quaternion.LookRotation(transform.forward, hit2.normal));
                    _animator.SetIKRotationWeight(avatarIKGoal, 1f);
                }
                if (groundDistance < diff) groundDistance = diff;
                return hit.point;
            }
            return Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_raypointLeft, 0.05f);
        Gizmos.DrawSphere(_raypointRight, 0.05f);
    }
}