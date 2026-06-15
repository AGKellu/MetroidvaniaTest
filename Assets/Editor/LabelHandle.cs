using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(DoorScript))]
public class LabelHandle : Editor
{
    private static GUIStyle labelStyle;
    private void OnEnable()
    {
        labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.black;
        labelStyle.alignment = TextAnchor.MiddleCenter;
    }
    
    private void OnSceneGUI()
    {
        DoorScript door = (DoorScript)target;

        Handles.BeginGUI();
        Handles.Label(door.transform.position + new Vector3(0f, .5f, 0f), door.gameObject.transform.GetChild(0).gameObject.GetComponent<TransitionScript>().CurrentDoorPosition.ToString(), labelStyle);
        Handles.EndGUI();
    }
}
