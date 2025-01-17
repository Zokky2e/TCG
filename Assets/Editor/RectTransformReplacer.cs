using UnityEditor;
using UnityEngine;

public class RectTransformReplacer : Editor
{
    [MenuItem("Tools/Replace Transform with RectTransform")]
    private static void ReplaceTransforms()
    {
        var selectedObjects = Selection.gameObjects;

        foreach (var obj in selectedObjects)
        {
            if (obj is GameObject selectedPrefab)
            {
                // Get the Transform component
                Transform transform = selectedPrefab.GetComponent<Transform>();

                // If it has a Transform but not a RectTransform, replace it
                if (transform != null && selectedPrefab.GetComponent<RectTransform>() == null)
                {
                    // Create a new GameObject to act as the UI version
                    GameObject tempUI = new GameObject(selectedPrefab.name);
                    RectTransform rectTransform = tempUI.AddComponent<RectTransform>();

                    // Copy the Transform's properties
                    rectTransform.localPosition = transform.localPosition;
                    rectTransform.localRotation = transform.localRotation;
                    rectTransform.localScale = transform.localScale;
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.sizeDelta = new Vector2(200, 200); // Default size (adjust if needed)

                    // Copy the child objects
                    foreach (Transform child in selectedPrefab.transform)
                    {
                        child.SetParent(tempUI.transform, false);
                    }

                    // Delete the old prefab and replace it
                    string assetPath = AssetDatabase.GetAssetPath(selectedPrefab);
                    GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(tempUI, assetPath);

                    // Destroy temporary object to avoid clutter
                    DestroyImmediate(tempUI);

                    Debug.Log($"Replaced Transform with RectTransform for {selectedPrefab.name}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Transform replacement completed!");
        }
    }

    [MenuItem("Tools/Update RectTransform Size to 200x200")]
    private static void UpdateRectTransformSizeTo200x200()
    {
        var selectedObjects = Selection.gameObjects;

        foreach (var selectedObject in selectedObjects)
        {
            // Get the RectTransform component
            RectTransform rectTransform = selectedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Update the sizeDelta to 200x200
                rectTransform.sizeDelta = new Vector2(200, 200);
                Debug.Log($"Updated RectTransform size of {selectedObject.name} to 200x200.");

                // Save the changes to the prefab
                PrefabUtility.SavePrefabAsset(selectedObject);
            }
            else
            {
                Debug.LogWarning($"{selectedObject.name} does not have a RectTransform component.");
            }
        }

        Debug.Log("Finished updating RectTransform sizes.");
    }
}
