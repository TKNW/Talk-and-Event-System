using System.Collections;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(NPCcontroller))]
public class NPCedtiorOption : Editor
{
    NPCcontroller Target;
    private bool _IsHavingMission;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Target = (NPCcontroller)target;
        _IsHavingMission = EditorGUILayout.Toggle("Has Mission", Target.HasMission);
        Target.HasMission = _IsHavingMission;
        EditorGUI.BeginDisabledGroup(_IsHavingMission == false);
        Target.MissionName = EditorGUILayout.TextField("MissionName", Target.MissionName);
        Target.MissionNumber = EditorGUILayout.IntField("Number",Target.MissionNumber);
        EditorGUI.EndDisabledGroup();
    }
}
