using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LaneGrid))]
public class LaneGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

        var grid = (LaneGrid)target;

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Regenerate Floor"))
            {
                grid.RegenerateFloor();
                EditorUtility.SetDirty(grid);
            }

            if (GUILayout.Button("Clear Generated"))
            {
                grid.ClearGenerated();
                EditorUtility.SetDirty(grid);
            }
        }
    }
}
