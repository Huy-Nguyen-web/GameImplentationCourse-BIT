#if UNITY_EDITOR

using UnityEditor;

public class MyCustomWindow : EditorWindow
{

    [MenuItem("Window")]
    public static void CreateWindow() => GetWindow<MyCustomWindow>("MyCustomWindow");

    private void OnGUI()
    {
        
    }
}
#endif
