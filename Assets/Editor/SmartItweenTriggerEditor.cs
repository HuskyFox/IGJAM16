using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom Inspector YEAH ohh YEAH!!
/// </summary>
[CustomEditor(typeof(SmartItweenObject)), CanEditMultipleObjects]
[System.Serializable]
public class SmartItweenTriggerEditor : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myScript = target as SmartItweenObject;

        if (myScript.AnimationType == SmartItweenObject.EAnimationType.PlayOnce)
        {
            //Reset to origin options
            myScript.ResetToOriginalPos = EditorGUILayout.Toggle("Reset to Origin", myScript.ResetToOriginalPos);
            if (myScript.ResetToOriginalPos)
            {
                myScript.ResetSpeed = EditorGUILayout.Slider("Return Speed", myScript.ResetSpeed, 0, 50);
                myScript.OriginalPosOffset = EditorGUILayout.Vector3Field("Original Position Offset", myScript.OriginalPosOffset);
                myScript.ToOriginEaseType = (SmartItweenObject.EaseType)EditorGUILayout.EnumPopup("Return EaseType", myScript.ToOriginEaseType);
            }

            //Unity Event
            myScript.CallEventOnFinish = EditorGUILayout.Toggle("CallEventOnFinish", myScript.CallEventOnFinish);
            if (myScript.CallEventOnFinish)
            {
                SerializedProperty OnPlayOnce = serializedObject.FindProperty("OnPlayOnceFinished");
                EditorGUIUtility.labelWidth = 25;
                EditorGUIUtility.fieldWidth = 50;
                EditorGUILayout.PropertyField(OnPlayOnce);
            }
        }

        //So that settings stay on play (in case of prefabs)
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}