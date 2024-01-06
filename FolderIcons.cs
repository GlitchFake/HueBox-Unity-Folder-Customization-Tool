using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;

namespace QuBit.HueBox
{
    [InitializeOnLoad]
    static class FolderIcons
    {
        static string selectedFolderGuid;
        static int controlID;

        static FolderIcons()
        {
            EditorApplication.projectWindowItemOnGUI -= OnGUI;
            EditorApplication.projectWindowItemOnGUI += OnGUI;
        }

        private static void OnGUI(string guid, Rect selectionRect)
        {
            if (guid != selectedFolderGuid)
                return;

            if (Event.current.commandName == "ObjectSelectorUpdated" &&
                EditorGUIUtility.GetObjectPickerControlID() == controlID)
            {
                UnityEngine.Object pickedObject = EditorGUIUtility.GetObjectPickerObject();
                string folderTextureGuid =
                    AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(pickedObject)).ToString();

                Debug.Log(guid);
                EditorPrefs.SetString(guid, folderTextureGuid);
            }
        }

        public static void SetCustomIconToFolder()
        {
            selectedFolderGuid = Selection.assetGUIDs[0];
            controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "", controlID);
            AssetDatabase.Refresh();
        }
    }
}

#endif