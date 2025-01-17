using UnityEngine;
using UnityEditor;

public class TextureImporterUtility : Editor
{
    [MenuItem("Tools/Set All Card Textures Readable and Trim")]
    private static void SetTexturesReadableAndTrim()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Playing Cards/Image/PlayingCards" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                // Set texture to readable
                importer.isReadable = true;
                AssetDatabase.ImportAsset(path);

                // Load the texture
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                // Trim the texture
                Rect trimmedRect = GetTrimmedRect(texture, 0.01f); // Adjust threshold as needed
                Texture2D trimmedTexture = TrimTexture(texture, trimmedRect);

                // Save the trimmed texture
                SaveTrimmedTexture(trimmedTexture, path);
            }
        }

        Debug.Log("All card textures have been set to readable and trimmed.");
    }

    private static Rect GetTrimmedRect(Texture2D texture, float alphaThreshold)
    {
        int width = texture.width;
        int height = texture.height;
        int xMin = width, xMax = 0, yMin = height, yMax = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (texture.GetPixel(x, y).a > alphaThreshold)
                {
                    xMin = Mathf.Min(xMin, x);
                    xMax = Mathf.Max(xMax, x);
                    yMin = Mathf.Min(yMin, y);
                    yMax = Mathf.Max(yMax, y);
                }
            }
        }

        return new Rect(xMin, yMin, xMax - xMin + 1, yMax - yMin + 1);
    }

    private static Texture2D TrimTexture(Texture2D texture, Rect rect)
    {
        Texture2D trimmedTexture = new Texture2D((int)rect.width, (int)rect.height);
        Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        trimmedTexture.SetPixels(pixels);
        trimmedTexture.Apply();
        return trimmedTexture;
    }

    private static void SaveTrimmedTexture(Texture2D texture, string path)
    {
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path);
    }
}
