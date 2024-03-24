using UnityEngine;

public class TransformGizmo : MonoBehaviour
{
    public bool isWire = false;
    public Color color;
    public float size = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (isWire)
        {
            Gizmos.DrawWireSphere(this.transform.position, size);
        }
        else
        {
            Gizmos.DrawSphere(this.transform.position, size);
        }
    }
}
