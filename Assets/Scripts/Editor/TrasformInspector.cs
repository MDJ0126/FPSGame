using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform), true)]
public class TransformInspector : Editor
{
    [System.Flags]
    enum Axes : byte
    {
        None = 0,
        X = 1 << 1,
        Y = 1 << 2,
        Z = 1 << 3,
        All = X | Y | Z,
    }

    Transform mTransform;
    SerializedProperty mPosition;
    SerializedProperty mRotation;
    SerializedProperty mScale;

    private void OnEnable()
    {
        mTransform = serializedObject.targetObject as Transform;
        mPosition = serializedObject.FindProperty("m_LocalPosition");
        mRotation = serializedObject.FindProperty("m_LocalRotation");
        mScale = serializedObject.FindProperty("m_LocalScale");
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 15f;
        serializedObject.Update();
        DrawPosition();
        DrawRotation();
        DrawScale();
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Position �׸���
    /// </summary>
    private void DrawPosition()
    {
        GUILayout.BeginHorizontal();
        bool reset = GUILayout.Button("P", GUILayout.Width(20f));
        EditorGUILayout.PropertyField(mPosition.FindPropertyRelative("x"));
        EditorGUILayout.PropertyField(mPosition.FindPropertyRelative("y"));
        EditorGUILayout.PropertyField(mPosition.FindPropertyRelative("z"));

        if (reset)
        {
            mPosition.vector3Value = Vector3.zero;
            CancelTextFocus();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Rotation �׸���
    /// </summary>
    private void DrawRotation()
    {
        GUILayout.BeginHorizontal();
        bool reset = GUILayout.Button("R", GUILayout.Width(20f));
        Vector3 visible = mTransform.localEulerAngles;

        visible.x = WrapAngle(visible.x);
        visible.y = WrapAngle(visible.y);
        visible.z = WrapAngle(visible.z);

        // ������ ���� ��ü �� ���� �ٸ� ���� ���� �����ϴ��� üũ
        Axes diff = CheckDifference(mRotation);

        // ���� �����ߴ��� �Ǵ��ϴµ� ����ϴ� ����
        Axes changed = Axes.None;

        float newX = FloatField("X", visible.x, (diff & Axes.X) != 0);
        if (newX != visible.x) changed |= Axes.X;

        float newY = FloatField("Y", visible.y, (diff & Axes.Y) != 0);
        if (newY != visible.y) changed |= Axes.Y;

        float newZ = FloatField("Z", visible.z, (diff & Axes.Z) != 0);
        if (newZ != visible.z) changed |= Axes.Z;

        if (changed != Axes.None)
        {
            Undo.RecordObjects(serializedObject.targetObjects, "Rotation");

            foreach (Object obj in serializedObject.targetObjects)
            {
                Transform t = obj as Transform;
                Vector3 v = t.localEulerAngles;

                if ((changed & Axes.X) != 0) v.x = newX;
                if ((changed & Axes.Y) != 0) v.y = newY;
                if ((changed & Axes.Z) != 0) v.z = newZ;

                t.localEulerAngles = v;
            }
        }

        if (reset)
        {
            mRotation.quaternionValue = Quaternion.identity;
            CancelTextFocus();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// ȸ���� �����ؼ� ��ȯ
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private float WrapAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }

    /// <summary>
    /// �ٸ� ���� �����ϴ��� üũ
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private Axes CheckDifference(SerializedProperty rotation)
    {
        Axes axes = Axes.None;

        // ������ ������ �Ǿ�����
        if (rotation.hasMultipleDifferentValues)
        {
            // ���ذ�
            Vector3 original = rotation.quaternionValue.eulerAngles;

            // �ٸ� ���� �����ϴ��� üũ
            foreach (Object obj in serializedObject.targetObjects)
            {
                Transform t = obj as Transform;
                Vector3 other = t.localEulerAngles;
                if (Mathf.Abs(other.x - original.x) > 0.0001f) axes |= Axes.X;
                if (Mathf.Abs(other.y - original.y) > 0.0001f) axes |= Axes.Y;
                if (Mathf.Abs(other.z - original.z) > 0.0001f) axes |= Axes.Z;
                if (axes == Axes.All) break;
            }
        }
        return axes;
    }

    /// <summary>
    /// Ŀ���� FloatField
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="hidden"></param>
    /// <returns></returns>
    private float FloatField(string name, float value, bool hidden)
    {
        if (hidden)
        {
            float newValue = value;
            GUI.color = new Color(0.75f, 0.75f, 0.75f);
            GUI.changed = false;
            float.TryParse(EditorGUILayout.TextField(name, "��"), out newValue);
            GUI.color = Color.white;
            if (GUI.changed) return newValue;
        }
        else
        {
            return EditorGUILayout.FloatField(name, value);
        }
        return value;
    }

    /// <summary>
    /// Sale �׸���
    /// </summary>
    private void DrawScale()
    {
        GUILayout.BeginHorizontal();
        bool reset = GUILayout.Button("S", GUILayout.Width(20f));
        EditorGUILayout.PropertyField(mScale.FindPropertyRelative("x"));
        EditorGUILayout.PropertyField(mScale.FindPropertyRelative("y"));
        EditorGUILayout.PropertyField(mScale.FindPropertyRelative("z"));

        if (reset)
        {
            mScale.vector3Value = Vector3.one;
            CancelTextFocus();
        }
        GUILayout.EndHorizontal();
    }

    private void CancelTextFocus() => GUIUtility.keyboardControl = 0;
}