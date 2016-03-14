using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(FastPool))]
public class FastPoolEditor : PropertyDrawer
{
    float propHeight = 30;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty sourcePrefab = property.FindPropertyRelative("sourcePrefab");
        Rect propRect = position;
        propRect.height = propHeight;

#if UNITY_4_5
        string displayName = "No Source Prefab";
#else
            string displayName = property.displayName;
#endif

        if (Application.isPlaying)
        {
            EditorGUI.LabelField(propRect, string.Concat(sourcePrefab.objectReferenceValue != null ? sourcePrefab.objectReferenceValue.name : displayName, " Pool"), EditorStyles.objectFieldThumb);

            propRect.height = 18;
            propRect.y += propHeight + 1;

            SerializedProperty spCached = property.FindPropertyRelative("cached_internal");
            SerializedProperty spCapacity = property.FindPropertyRelative("Capacity");
            
            EditorGUI.ProgressBar(propRect, (float)spCached.intValue / spCapacity.intValue, string.Concat(spCached.intValue.ToString(), "/", spCapacity.intValue > 0 ? spCapacity.intValue.ToString() : "Unlimited"));
        }
        else
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;

            //Draw custom foldout
            if (GUI.Button(propRect, sourcePrefab.objectReferenceValue != null ? sourcePrefab.objectReferenceValue.name : displayName, EditorStyles.objectFieldThumb))
                property.isExpanded = !property.isExpanded;
            if (property.isExpanded)
            {
                //Draw background
                Rect bgRect = new Rect(position.x, position.y + propHeight, position.width, position.height - propHeight);
                EditorGUI.HelpBox(bgRect, "", MessageType.None);

                //Draw sourcePrefab
                property = sourcePrefab;
                propRect.width -= 4;
                propRect.y += 4 + propHeight;
                EditorGUI.PropertyField(propRect, property);

                //Draw Capacity
                property.NextVisible(false);
                propRect.y += propHeight + 1;
                if (property.intValue < 0)
                    property.intValue = 0;
                EditorGUI.PropertyField(propRect, property);

                //Draw Preload
                int rightValue = property.intValue;
                property.NextVisible(false);
                propRect.y += propHeight + 1;
                if (rightValue > 0)
                {
                    if (property.intValue > rightValue)
                        property.intValue = rightValue;
                    EditorGUI.IntSlider(propRect, property, 0, rightValue);
                }
                else
                    EditorGUI.PropertyField(propRect, property);

                //Draw Notification Type and UseSceneClone
                for (int i = 0; i < 3; i++)
                {
                    property.NextVisible(false);
                    propRect.y += propHeight + 1;
                    EditorGUI.PropertyField(propRect, property);
                }
            }

            EditorGUI.indentLevel = indent;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        propHeight = base.GetPropertyHeight(property, label);

        if (Application.isPlaying)
            return (propHeight + (property.isExpanded ? 2 : 0)) * 2;
        else
            return (propHeight + (property.isExpanded ? 2 : 0)) * property.CountInProperty();
    }
}
