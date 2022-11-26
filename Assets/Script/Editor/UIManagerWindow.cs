using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerWindow : EditorWindow
{
    string primaryTag = "User Interface - Primary";
    Sprite primarySprite;
    Color primaryColor;
    [MenuItem("Window/UI Manager")]
    public static void ShowWindow()
    {
        GetWindow<UIManagerWindow>("UI Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Classification", EditorStyles.label);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Primary Tag", GUILayout.MaxWidth(125));
        if (GUILayout.Button(primaryTag))
        {
            foreach(GameObject i in Selection.gameObjects)
            {
                i.gameObject.tag = primaryTag;
            }
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.Label("Settings", EditorStyles.label);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Primary Image", GUILayout.MaxWidth(125));
        primarySprite = (Sprite)EditorGUILayout.ObjectField(primarySprite, typeof(Sprite), true);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Primary Color", GUILayout.MaxWidth(125));
        primaryColor = EditorGUILayout.ColorField(primaryColor);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (GUILayout.Button("Apply"))
        {
            foreach(GameObject i in GameObject.FindGameObjectsWithTag(primaryTag))
            {
                i.GetComponent<Image>().sprite = primarySprite;
                i.GetComponent<Image>().color = primaryColor;
            }
        }

    }


}
