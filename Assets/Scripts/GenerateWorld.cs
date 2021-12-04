using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.IO;

public class GenerateWorld : MonoBehaviour
{
    private GameObject terrain;
    private NoiseMapGeneration terrianGenerator;
    private EnvironmentGenerator envGen;
    private GameObject Player;
    private EnemyGenerator enemyGen;

    private int mapSize;
    private int size = 256;
    private float noiseScale = 10;
    private int mapDepth = 3;
    private float noiseScaleChange = 0.2f;
    private float mapDepthChange = 0.1f;
    private int sizeChange = 10;
    private int currentSize;

    private int decor1Count = 150;
    private int decor2Count = 50;
    private int decor3Count = 40;
    private int decor1CountChange = 5;
    private int decor2CountChange = 3;
    private int decor3CountChange = 2;

    private int playerHealth = 300;
    private int playerHealthChange = 10;

    public int level = 1;
    public int score = 0;
    public int enemyCount = 5;

    private float enemyCountChange = 0.3f;

    public GameInfo gameInfo;


    // Start is called before the first frame update
    void Awake()
    {
        // Defining some vars
        currentSize = (size + level * sizeChange < 512 - 50) ? size + level * sizeChange : 512 - 50;
        mapSize = 512;

        terrain = GameObject.FindGameObjectWithTag("Terrain");
        terrianGenerator = terrain.GetComponent<NoiseMapGeneration>();
        Player = GameObject.FindGameObjectWithTag("Player");
        envGen = GetComponent<EnvironmentGenerator>();
        enemyGen = GameObject.FindGameObjectWithTag("Enemy Generator").GetComponent<EnemyGenerator>();

        float k = (float)mapSize / (float)size;
        noiseScale *= k;

        ReadLevelFile();

        // Generating Terrain
        Player.transform.position = new Vector3(mapSize / 2, mapDepth + Mathf.RoundToInt(mapDepthChange * level) + 1, mapSize / 2); // Setting Player to the center of terrain

        CreateEdges(); // Creating Edges of the world so Player can't go outside box 

        // Applying Terrain Generator Parameters
        terrianGenerator.width = mapSize;
        terrianGenerator.height = mapSize;
        terrianGenerator.scale = noiseScale + noiseScaleChange * level;
        terrianGenerator.depth = mapDepth + Mathf.RoundToInt(mapDepthChange * level);

        terrianGenerator.Generate(); // Generating Terrain
        terrain.isStatic = true;

        // Generating Environment
        // Applying Environment Generator parameters
        envGen.x1 = mapSize / 2 - currentSize / 2;
        envGen.x2 = mapSize / 2 + currentSize / 2;
        envGen.y = mapDepth + Mathf.RoundToInt(mapDepthChange * level) + 2;

        envGen.GetDecor2Sizes(); // Getting the actual heights of cactuses
        envGen.GenerateDecor1s(decor1Count + decor1CountChange * level); // Adding Decor1
        envGen.GenerateDecor2s(decor2Count + decor2CountChange * level); // Adding Decor2
        envGen.GenerateDecor3s(decor3Count + decor3CountChange * level); // Adding Decor3

        // Generating Enemies
        // Applying Enemy Generator parameters
        enemyGen.count = enemyCount + Mathf.RoundToInt(enemyCountChange * level);
        enemyGen.height = mapDepth + Mathf.RoundToInt(mapDepthChange * level);
        enemyGen.terrainSize = mapSize;
        enemyGen.distance = currentSize;

        // Applying Player's health
        Player.GetComponent<CarHealth>().health = playerHealth + playerHealthChange * level;
    }

    // Create a box collider
    private void CreateEdge(Vector3 pos, Vector3 size, Vector3 rot)
    {
        GameObject side = new GameObject();
        side.transform.position = pos;
        side.transform.rotation = Quaternion.Euler(rot);
        side.AddComponent<BoxCollider>();
        BoxCollider col = side.GetComponent<BoxCollider>();
        col.size = size;
    }

    // Create 4 edges to avoid being car outside the area
    private void CreateEdges()
    {
        CreateEdge(new Vector3(mapSize / 2, 0, mapSize / 2 - currentSize / 2), new Vector3(currentSize, currentSize, 10), Vector3.zero);
        CreateEdge(new Vector3(mapSize / 2, 0, mapSize / 2 + currentSize / 2), new Vector3(currentSize, currentSize, 10), Vector3.zero);
        CreateEdge(new Vector3(mapSize / 2 - currentSize / 2, 0, mapSize / 2), new Vector3(currentSize, currentSize, 10), new Vector3(0, 90, 0));
        CreateEdge(new Vector3(mapSize / 2 + currentSize / 2, 0, mapSize / 2), new Vector3(currentSize, currentSize, 10), new Vector3(0, 90, 0));
        CreateEdge(new Vector3(currentSize, 50 + mapDepth + Mathf.RoundToInt(mapDepthChange * level), currentSize), new Vector3(currentSize, currentSize, 10), new Vector3(90, 0, 0));
    }

    // Find Level and Score Texts and set them to needed properties
    private void Start()
    {
        GameObject.FindGameObjectWithTag("Level").GetComponent<Text>().text = $"LEVEL: {level}";
        GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text = $"SCORE: {score}";
    }

    // Write GameInfo class to JSON file
    public void WriteLevelFile(GameInfo GI)
    {
        string fileName = Path.Combine(Application.streamingAssetsPath,"gameInfo.json");
        string jsonString = JsonUtility.ToJson(GI);
        File.WriteAllText(fileName, jsonString);
    }

    // Read a JSON file (by GameInfo class)
    public void ReadLevelFile()
    {
        string jsonString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "gameInfo.json"));
        Debug.Log($"JSON READ -> {jsonString}");
        gameInfo = JsonUtility.FromJson<GameInfo>(jsonString);
        Debug.Log($"JSON READ GAMEINFO -> {gameInfo.Level}, {gameInfo.Score}");
        level = gameInfo.Level;
        score = gameInfo.Score;
    }
}

// This class helps to get and store data from/in JSON file
public class GameInfo
{
    public int Level;
    public int Score;
}