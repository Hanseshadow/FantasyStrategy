#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ResetGameObjectMacro
{
    [MenuItem("GameObject/Selection/Reset %#r")]
    public static void SelectGroup()
    {
        if(!ValidSelection()) return;

        for(int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if(Selection.gameObjects[i] == null)
            {
                continue;
            }

            Selection.gameObjects[i].transform.localPosition = Vector3.zero;
            Selection.gameObjects[i].transform.localScale = Vector3.one;
            Selection.gameObjects[i].transform.eulerAngles = Vector3.zero;
        }
    }

    private static bool ValidSelection()
    {
        if(Selection.activeTransform == null)
        {
            Debug.Log("Macro Error: First select a game object.");
            return false;
        }

        return true;
    }
}
#endif