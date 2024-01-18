using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

namespace QuBit.HueBox
{
    [InitializeOnLoad]
    static class PaintFolder
    {
        private static Material folderMaterial;
        const string FolderColorKey = "_Color";
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

            string iconPath = "Assets/Editor/HueBox/Icons/" + m_iconName + ".png";
            string iconGuid = AssetDatabase.GUIDFromAssetPath(iconPath).ToString();

            EditorPrefs.SetString(folderGuid, iconGuid);
        }

        public static void SetFolderColor(Color folderColor)
        {
            // Aktif nesnenin bir klas�r olup olmad���n� kontrol et
            if (Selection.activeObject != null && AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(Selection.activeObject)))
            {
                // Klas�r yolunu ve GUID'ini al
                string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                string folderGuid = AssetDatabase.GUIDFromAssetPath(folderPath).ToString();

                // Renk bilgisini kaydet
                EditorPrefs.SetString(folderGuid + FolderColorKey, ColorUtility.ToHtmlStringRGBA(folderColor));
                
                // Klas�r rengini uygula
                ApplyColorToFolder(folderPath, folderColor);
            }
            else
            {
                Debug.LogWarning("Please select a valid folder.");
            }
        }

        private static void ApplyColorToFolder(string folderPath, Color folderColor)
        {
            // Klas�r alt�ndaki dosyalar� i�lemek yerine, sadece klas�r rengini de�i�tir
            Texture2D folderTexture = new Texture2D(16, 16);

            // T�m pikselleri istedi�iniz renge ayarlay�n
            Color[] pixels = new Color[folderTexture.width * folderTexture.height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = folderColor;
            }

            folderTexture.SetPixels(pixels);
            folderTexture.Apply();

            // Klas�r i�in GUID al�n�r
            string folderGuid = AssetDatabase.AssetPathToGUID(folderPath);

            // Klas�r i�in bir icon olu�turup EditorPrefs ile kaydedin
            string folderIconPath = "Assets/Editor/HueBox/FolderIcons/" + folderGuid + "_FolderIcon.png";
            File.WriteAllBytes(folderIconPath, folderTexture.EncodeToPNG());

            // Olu�turulan ikonun GUID'sini al�n
            string iconGuid = AssetDatabase.AssetPathToGUID(folderIconPath);

            // Texture y�kleme i�lemi ba�ar�s�z olduysa (dosya bulunamad�), uyar� ver ve i�lemi sonland�r
            if (string.IsNullOrEmpty(iconGuid))
            {
                Debug.LogError("Icon GUID is null or empty.");
                return;
            }

            EditorGUIUtility.SetIconSize(new Vector2(16, 16));

            // EditorGUIUtility.SetIconForObject metodu i�in bir Texture2D nesnesine ihtiya� var
            // Bu nedenle, yukar�da olu�turulan folderTexture'� kullanarak bir nesne yarat�lmal�d�r
            Texture2D folderTextureLoaded = new Texture2D(1, 1);
            folderTextureLoaded.LoadImage(File.ReadAllBytes(folderIconPath));

            // Texture y�kleme i�lemi ba�ar�s�z olduysa (dosya bulunamad�), uyar� ver ve i�lemi sonland�r
            if (folderTextureLoaded == null)
            {
                Debug.LogError("Texture is null for GUID: " + iconGuid);
                return;
            }

            EditorPrefs.SetString(folderGuid, folderIconPath);

            // Klas�r i�in simgeyi ayarla
            EditorGUIUtility.SetIconForObject(
            AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(folderPath),
            AssetDatabase.LoadAssetAtPath<Texture2D>(folderIconPath)
            );
            EditorGUIUtility.SetIconSize(Vector2.zero);
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
