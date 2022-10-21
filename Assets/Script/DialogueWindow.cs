using UnityEditor;
using UnityEngine;

public class DialogueWindow : EditorWindow
{
    [MenuItem("Window/DialogueWindow")]


    static void InitWindow()
    {
        DialogueWindow window = GetWindow<DialogueWindow>();
        window.titleContent = new GUIContent("Dialogue Window");
        window.Show();
    }
}
