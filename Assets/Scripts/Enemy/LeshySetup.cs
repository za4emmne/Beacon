#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class LeshySetup : MonoBehaviour
{
    [MenuItem("GameObject/Setup Leshy Prefab", false, 10)]
    public static void SetupLeshyPrefab()
    {
        // Get the Leshiy prefab
        string prefabPath = "Assets/Prefabs/Enemy/Leshiy.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        if (prefab == null)
        {
            Debug.LogError("Leshiy prefab not found at: " + prefabPath);
            return;
        }
        
        // Check if Leshy component already exists
        Leshy leshy = prefab.GetComponent<Leshy>();
        if (leshy == null)
        {
            leshy = prefab.AddComponent<Leshy>();
            Debug.Log("Added Leshy component to Leshiy prefab");
        }
        
        // Ensure required components exist
        if (prefab.GetComponent<EnemyHealth>() == null)
        {
            prefab.AddComponent<EnemyHealth>();
            Debug.Log("Added EnemyHealth component to Leshiy prefab");
        }
        
        if (prefab.GetComponent<EnemyAttacked>() == null)
        {
            prefab.AddComponent<EnemyAttacked>();
            Debug.Log("Added EnemyAttacked component to Leshiy prefab");
        }
        
        // Create a spawn point if it doesn't exist
        Transform spawnPoint = prefab.transform.Find("BatSpawnPoint");
        if (spawnPoint == null)
        {
            GameObject spawnPointObj = new GameObject("BatSpawnPoint");
            spawnPointObj.transform.SetParent(prefab.transform);
            spawnPointObj.transform.localPosition = Vector3.zero;
            spawnPoint = spawnPointObj.transform;
            Debug.Log("Created BatSpawnPoint on Leshiy prefab");
        }
        
        // Try to assign LeshyData if it exists
        LeshyData leshyData = AssetDatabase.LoadAssetAtPath<LeshyData>("Assets/Scripts/Enemy/LeshyData.asset");
        if (leshyData != null && leshy != null)
        {
            // We can't set serialized fields in edit mode easily, but we can log that it should be set
            Debug.Log("LeshyData found. Please assign it to the Leshy component in the prefab.");
        }
        else
        {
            Debug.LogWarning("LeshyData asset not found. Create it via Assets/Create/Enemy/Create new Leshy");
        }
        
        // Save the prefab
        EditorUtility.SetDirty(prefab);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Leshy prefab setup complete!");
    }
}
#endif