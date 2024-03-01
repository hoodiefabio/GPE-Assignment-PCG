using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractGenerator),true)]
public class DungeonGenEditor : Editor
{
   AbstractGenerator generator;

    private void Awake()
    {
        generator = (AbstractGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
