using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddRawImageComponent : Editor
{
    [MenuItem("Tools/Add Image Component to Prefabs")]
    private static void AddImageToPrefabs()
    {
        var selectedObjects = Selection.gameObjects;
        foreach (var selectedObject in selectedObjects)
        {
            // Add RawImage component if not already present
            RawImage frontImage = selectedObject.GetComponent<RawImage>();
            if (frontImage != null)
            {
                // If it has a RawImage, rename it to FrontImage
                frontImage.name = "FrontImage";
            }
            else
            {
                // If no RawImage, add one and rename it to FrontImage
                frontImage = selectedObject.AddComponent<RawImage>();
                frontImage.name = "FrontImage";
            }
            // Now create a child GameObject for the BackImage
            GameObject backImageObject = new GameObject("BackImage");
            backImageObject.transform.SetParent(selectedObject.transform); // Set the parent to the card prefab
            backImageObject.transform.localPosition = Vector3.zero; // Reset position to match the parent
            
            // Add a RawImage to the child GameObject
            RawImage backImage = backImageObject.AddComponent<RawImage>();
            backImage.name = "BackImage"; 
            backImageObject.SetActive(false);
            // Extract the part of the name between "PlayingCards_" and the last underscore "_"
            string prefabName = selectedObject.name;
            int startIndex = prefabName.IndexOf("PlayingCards_") + "PlayingCards_".Length;
            int endIndex = prefabName.LastIndexOf("_");

            if (startIndex >= 0 && endIndex > startIndex)
            {
                string imageName = prefabName.Substring(startIndex, endIndex - startIndex);
                if (imageName != "FrontImage")
                {
                    string frontImagePath = $"Assets/Playing Cards/Image/PlayingCards/{imageName}.png";
                    Texture2D frontTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(frontImagePath);

                    if (frontTexture != null)
                    {
                        frontImage.texture = frontTexture;
                        Debug.Log($"Assigned FrontImage {imageName} to {selectedObject.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Front image not found for {imageName} at {frontImagePath}");
                    }
                }

            }
            else
            {
                Debug.LogWarning($"Could not extract image name from {prefabName}");
            }
            string backImagePath = $"Assets/Playing Cards/Image/PlayingCards/BackColor_Red.png";
            Texture2D backTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(backImagePath);

            if (backTexture != null)
            {
                backImage.texture = backTexture;
            }

            // Save changes to the prefab
            PrefabUtility.SavePrefabAsset(selectedObject);
        }

        Debug.Log("Finished adding Image components.");
    }
}
