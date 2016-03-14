using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(FastPoolManager))]
public class FastPoolManagerEditor : Editor
{
    SerializedProperty pools;
    FastPoolManager fpm;


    void OnEnable()
    {
        pools = serializedObject.FindProperty("predefinedPools");
        fpm = (FastPoolManager)target;
    }


    public override void OnInspectorGUI()
    {
        GUILayout.Space(5);

        if (EditorApplication.isPlaying)
        {
            foreach (KeyValuePair<int, FastPool> pool in fpm.Pools)
            {
                if (pool.Value.IsValid)
                {
                    EditorGUILayout.LabelField(pool.Value.Name, EditorStyles.objectFieldThumb);

                    Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
                    EditorGUI.ProgressBar(rect, (float)pool.Value.Cached / pool.Value.Capacity, string.Concat(pool.Value.Cached.ToString(), "/",  pool.Value.Capacity > 0 ? pool.Value.Capacity.ToString() : "Unlimited"));
                    EditorGUILayout.Space();
                }
            }
        }
        else
        {
            serializedObject.Update();

            EditorGUILayout.Space();

            for (int i = 0; i < pools.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(pools.GetArrayElementAtIndex(i), false);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(20)))
                    pools.DeleteArrayElementAtIndex(i);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(2);

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Add New Pool"))
                pools.InsertArrayElementAtIndex(pools.arraySize);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

