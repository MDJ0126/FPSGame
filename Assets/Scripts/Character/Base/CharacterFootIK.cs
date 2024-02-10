using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(TwoBoneIKConstraint))]
public class CharacterFootIK : MonoBehaviour
{
    #region Inspector

    public Vector3 offset;

    #endregion

    public TwoBoneIKConstraint TwoBoneIKConstraint { get; private set; } = null;

    private void Awake()
    {
        this.TwoBoneIKConstraint = GetComponent<TwoBoneIKConstraint>();
    }

    private void FixedUpdate()
    {
        this.TwoBoneIKConstraint.weight = 0f;
        Vector3 start = this.TwoBoneIKConstraint.data.tip.position + offset;
        Vector3 dir = Vector3.down;
        const float DISTANCE = 1f;
        Debug.DrawRay(start, dir * DISTANCE);
        if (Physics.Raycast(start, dir, out RaycastHit hit, DISTANCE))
        {
            this.TwoBoneIKConstraint.data.target.position = hit.point + offset;
            float diff = Vector3.Distance(hit.point, start);
            Debug.Log(diff);
            if (diff <= 0.25f)
            {
                this.TwoBoneIKConstraint.weight = 1f;
            }
        }
    }

    public void Move(Vector3 wolrdPosition)
    {
    }

    private void OnDrawGizmos()
    {
        //if (!Application.isPlaying) return;
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(_oldPosition, 0.2f);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(NewPosition, 0.2f);
    }
}
