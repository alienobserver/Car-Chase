using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    public int count;
    public int terrainSize;
    public float height = 10f;
    public int distance = 25;
    public int scoreNum;

    public int startScore;

    private Text EnemyText;

    public UpdateStatus StatusUpdater;

    public int scorePoint = 50;

    private Text score;

    private int countAll;

    public NavMeshSurface NavMeshSurfaceTerrain;

    // Start is called before the first frame update
    void Start()
    {
        EnemyText = GameObject.FindGameObjectWithTag("Enemy Number").GetComponent<Text>();
        EnemyText.text = "Enemies Remained: " + count;

        score = GameObject.Find("Score").GetComponent<Text>();

        StatusUpdater = GameObject.FindGameObjectWithTag("Status").GetComponent<UpdateStatus>();

        startScore = GameObject.FindGameObjectWithTag("Status").GetComponent<GenerateWorld>().gameInfo.Score;

        NavMeshSurfaceTerrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<NavMeshSurface>();

        BuildNavMesh();
        CreateEnemies();
    }

    private void BuildNavMesh()
    {
        NavMeshSurfaceTerrain.RemoveData();
        NavMeshSurfaceTerrain.BuildNavMesh();
    }

    private void UpdateText()
    {
        if (EnemyText && count >= 0) EnemyText.text = $"ENEMIES REMAINED: {count}";
        scoreNum = startScore + (countAll - count) * scorePoint;
        if (score) score.text = $"SCORE: {scoreNum}";
    }

    private void CreateEnemies()
    {
        int countPrefab = 0;
        countAll = count;

        for (int i = 0; i < prefabs.Length; i++)
        {
            countAll -= countPrefab;

            if (i < prefabs.Length - 1) countPrefab = Random.Range(1, (countAll) - (prefabs.Length - i + 1));
            else countPrefab = countAll;

            Debug.Log($"{i} -> {countPrefab}");

            for (int j = 0; j < countPrefab; j++)
            {
                Instantiate(prefabs[i], new Vector3(Random.Range(terrainSize / 2 - distance / 2 + 20, terrainSize / 2 + distance / 2 - 20), height, Random.Range(terrainSize / 2 - distance / 2 + 20, terrainSize / 2 + distance / 2 - 20)), Quaternion.identity);
            }
        }

        countAll = count;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
        if (count == 0)
        {
            StatusUpdater.worldGen.score = scoreNum;
            StartCoroutine(StatusUpdater.LoadWinScene(1));
            count--;
        }
    }
}
