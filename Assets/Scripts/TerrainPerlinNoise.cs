using UnityEditor;
using UnityEngine;
using System.Collections;

public class TerrainPerlinNoise : ScriptableWizard
{

    public float Tiling = 10.0f;

    [MenuItem("Terrain/Generate from Perlin Noise")]
    public static void CreateWizard(MenuCommand command)
    {
        ScriptableWizard.DisplayWizard("Perlin Noise Generation Wizard", typeof(TerrainPerlinNoise));
    }

    void OnWizardUpdate()
    {
        helpString = "This small generation tool allows you to generate perlin noise for your terrain.";
    }

    void OnWizardCreate()
    {
        GameObject obj = Selection.activeGameObject;

        if (obj.GetComponent<Terrain>())
        {
            GenerateHeights(obj.GetComponent<Terrain>(), Tiling);
        }
    }

    public void GenerateHeights(Terrain terrain, float tileSize)
    {
        float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
            {
                heights[i, k] = Mathf.PerlinNoise(((float)i / (float)terrain.terrainData.heightmapWidth) * tileSize, ((float)k / (float)terrain.terrainData.heightmapHeight) * tileSize) / 10.0f;
            }
        }

        terrain.terrainData.SetHeights(0, 0, heights);
    }
}