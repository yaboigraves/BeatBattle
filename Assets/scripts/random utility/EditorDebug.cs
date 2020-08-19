using UnityEngine;
using UnityEditor;

public class EditorDebug : EditorWindow
{
    //public Player player;

    [MenuItem("Window/EditorDebug")]
    public static void ShowWindow()
    {
        GetWindow<EditorDebug>("EditorDebug");
    }
    private void OnGUI()
    {
        if (GUILayout.Button("Flip Player"))
        {
            FunctionToRun();
        }


    }
    private void FunctionToRun()
    {
        Debug.Log("The Function ran");
        //GameObject.FindObjectOfType<Player>().flip(180);
    }
}
