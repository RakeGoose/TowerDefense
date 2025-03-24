using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public LevelManager LM;
    gameController gcontroller;

    public float timeBetweenWaves = 5f;
    public float timeBetweenEnemies = 0.4f;

    public GameObject enemyPrefab;

    public int enemiesPerWave = 3;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    private void Start()
    {
        gcontroller = FindObjectOfType<gameController>();
        Debug.Log("EnemySpawner Started");
    }

    public void StartSpawning()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (GameManagerScript.Instance.canSpawn)
        {
            GameManagerScript.Instance.waveCount++;
            int currentWave = GameManagerScript.Instance.waveCount;

            GameManagerScript.Instance.ShowWaveStart(currentWave);

            Debug.Log($"Wave {currentWave} started");
            isSpawning = true;

            for(int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                enemiesAlive++;
                yield return new WaitForSeconds(timeBetweenEnemies);
            }

            isSpawning = false;

            yield return new WaitUntil(() => enemiesAlive <= 0);

            GameManagerScript.Instance.OnWaveComplete();

            enemiesPerWave += 2;

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {

        GameObject tmpEnemy = Instantiate(enemyPrefab);
        tmpEnemy.transform.SetParent(gameObject.transform, false);

        tmpEnemy.GetComponent<EnemyLogic>().selfEnemy = gcontroller.AllEnemies[Random.Range(0, gcontroller.AllEnemies.Count)];
        ;
        Transform startCellPos = LM.wayPoints[0].transform;
        Vector3 startPos = new Vector3(startCellPos.position.x + startCellPos.GetComponent<SpriteRenderer>().bounds.size.x / 2,
                                       startCellPos.position.y + startCellPos.GetComponent<SpriteRenderer>().bounds.size.y);

        tmpEnemy.transform.position = startPos;

    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }

}
