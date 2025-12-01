#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerAttack))]
public class MyCustomEditor : Editor
{
    SerializedProperty attackSpeedProp;
    private void OnEnable()
    {
        attackSpeedProp = serializedObject.FindProperty("recoveryTime");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Slider(attackSpeedProp, 0, 2, new GUIContent("Attack Delay"));
    }
}
#endif
