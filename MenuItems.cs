using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace QuBit.HueBox
{
    static class MenuItems
    {
        const int menuItemPriority = 10000;

        [MenuItem("Assets/HueBox/Red", false, menuItemPriority + 1)]
        static void Red()
        {
            PaintFolder.SetIconName("Red");
        }

        [MenuItem("Assets/HueBox/Green", false, menuItemPriority + 2)]
        static void Green()
        {
            PaintFolder.SetIconName("Green");
        }

        [MenuItem("Assets/HueBox/Blue", false, menuItemPriority + 3)]
        static void Blue()
        {
            PaintFolder.SetIconName("Blue");
        }
        class EditorGUIColorField : EditorWindow
        {
            Color matColor = Color.white;
            [MenuItem("Assets/HueBox/Set Color/Custom", false, menuItemPriority + 10)]
            static void OpenColorPicker()
            {
                // EditorGUIColorField sýnýfýndan bir pencere oluþturun
                var window = GetWindow<EditorGUIColorField>();

                // Pencerenin pozisyonunu ve boyutunu belirtin
                window.position = new Rect(500, 400, 170, 60);

                // Pencereyi gösterin
                window.Show();
            }
            void OnGUI()
            {
                // Renk seçimi için bir EditorGUI.ColorField oluþturun
                matColor = EditorGUI.ColorField(new Rect(3, 3, position.width - 6, 15),
                    "New Color:", // Renk seçim alanýnýn etiketi
                    matColor);    // Varsayýlan renk deðeri

                // "Change!" adýnda bir düðme oluþturun
                if (GUI.Button(new Rect(3, 25, position.width - 6, 30), "Change!"))
                {
                    Debug.Log("Button Clicked!");
                    Debug.Log("Selected Color: " + matColor.ToString());
                    PaintFolder.SetFolderColor(matColor);
                }
            }

        }

        [MenuItem("Assets/HueBox/Custom Color", false, menuItemPriority + 10)]
        static void CustomColor()
        {
            Debug.Log("Custom Color");
            //HueBox.SetColor(Color.red);
        }

        [MenuItem("Assets/HueBox/Set Icon", false, menuItemPriority + 11)]
        static void SetCustomIcon()
        {
            FolderIcons.SetCustomIconToFolder();
        }

        [MenuItem("Assets/HueBox/Reset Icon", false, menuItemPriority + 23)]
        static void ResetCustomIcon()
        {
            PaintFolder.ResetFolderTexture();
        }


        [MenuItem("Assets/HueBox/Red", true)]
        [MenuItem("Assets/HueBox/Green", true)]
        [MenuItem("Assets/HueBox/Blue", true)]
        [MenuItem("Assets/HueBox/Custom Color", true)]
        [MenuItem("Assets/HueBox/Set Icon", true)]
        [MenuItem("Assets/HueBox/Reset Icon", true)]
        static bool ValidateFolder()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }
            Object selectedObject = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(selectedObject);
            return AssetDatabase.IsValidFolder(path);
        }
    }
}


#endif