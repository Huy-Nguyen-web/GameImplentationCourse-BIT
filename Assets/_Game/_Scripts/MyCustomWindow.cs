#if UNITY_EDITOR

using UnityEditor;

public class MyCustomWindow : EditorWindow
{
    string testString = "Hellooo!";
    bool testBool;
    float testFloat = 2.5f;

    [MenuItem("Window/MyCustomWindow")]
    public static void CreateWindow() => GetWindow<MyCustomWindow>("MyCustomWindow");

    private void OnGUI()
    {
        testString = EditorGUILayout.TextField("Text Field", testString);
        testBool = EditorGUILayout.Toggle("Toggle", testBool);
        testFloat = EditorGUILayout.Slider("Slider", testFloat, 0, 10);
    }
}
#endif
