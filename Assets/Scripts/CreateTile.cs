using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateTile
{
    [MenuItem("Assets/Create/2D/Tiles/Basic Tile (Manual)")]
    public static void CreateBasicTile()
    {
        var tile = ScriptableObject.CreateInstance<Tile>();
        AssetDatabase.CreateAsset(tile, "Assets/NewTile.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = tile;
    }
}