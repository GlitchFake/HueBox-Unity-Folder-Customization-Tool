using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;

namespace QuBit.HueBox
{
    [InitializeOnLoad]
    static class PaintFolder
    {
        static PaintFolder()
        {
            EditorApplication.projectWindowItemOnGUI -= OnGUI;
            EditorApplication.projectWindowItemOnGUI += OnGUI;
        }
        private static void OnGUI(string guid, Rect selectionRect)
        {
            Color bgColor;
            Rect folderRect = GetFolderRect(selectionRect, out bgColor);

            string iconGuid = EditorPrefs.GetString(guid, "");

            if (iconGuid is "" or null)
                return;


            EditorGUI.DrawRect(folderRect, bgColor);

            string folderTexturePath = AssetDatabase.GUIDToAssetPath(iconGuid);
            Texture2D folderTexture =
                AssetDatabase.LoadAssetAtPath<Texture2D>(folderTexturePath);
            GUI.DrawTexture(folderRect, folderTexture);
        }

        private static Rect GetFolderRect(Rect selectionRect, out Color backgroundColor)
        {
            Rect folderRect;
            backgroundColor = new Color(.2f, .2f, .2f, 0);

            if (selectionRect.x < 15)
            {
                folderRect = new Rect(selectionRect.x + 3, selectionRect.y, selectionRect.height, selectionRect.height);
            }
            else if (selectionRect.x >= 15 && selectionRect.height < 30)
            {
                folderRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height);
                backgroundColor = new Color(.15f, .15f, .15f, 0);
            }
            else
            {
                folderRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width,
                    selectionRect.width);
            }

            return folderRect;
        }

        public static void SetIconName(string m_iconName)
        {
            string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string folderGuid = AssetDatabase.GUIDFromAssetPath(folderPath).ToString();

            string iconPath = "Assets/HueBox/Icons/" + m_iconName + ".png";
            string iconGuid = AssetDatabase.GUIDFromAssetPath(iconPath).ToString();

            EditorPrefs.SetString(folderGuid, iconGuid);
        }

        public static void ResetFolderTexture()
        {
            string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string folderGuid = AssetDatabase.GUIDFromAssetPath(folderPath).ToString();

            EditorPrefs.DeleteKey(folderGuid);
            AssetDatabase.Refresh();
        }


    }
}
#endif