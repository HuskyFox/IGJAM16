using ObjectPooling;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPoolManager))]
public class ObjectPoolManagerEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ObjectPoolManager myScript = (ObjectPoolManager) target;
        DrawAddPoolButton(myScript);
    }

    private static void DrawAddPoolButton(ObjectPoolManager myScript)
    {
        if (GUILayout.Button("Add Pool"))
        {
            if (!myScript.ObjectToPool)
                EditorUtility.DisplayDialog("Oops!",
                    "Which object do you want to pool? Make sure you assign one in the inspector", "OK!");

            myScript.RefreshPoolList();

            if (myScript.PoolExists(myScript.ObjectToPool))
                EditorUtility.DisplayDialog("Oops!", "A pool of " + myScript.ObjectToPool.name + " already exists", "OK");

            myScript.CreateNewPool(myScript.ObjectToPool, myScript.ObjectsPreloaded, myScript.PoolCanGrow);
        }
    }
}

