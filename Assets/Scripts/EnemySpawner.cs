using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public LevelManager LM;
    public float timeToSpawn = 4;
    int spawnCount = 0;

    public GameObject enemyPrefab;

    void Update()
    {
        if (timeToSpawn <= 0)
        {
            StartCoroutine(SpawnEnemy(spawnCount + 1));
            timeToSpawn = 4;
        }

        timeToSpawn -= Time.deltaTime;
    }

    IEnumerator SpawnEnemy(int enemyCount)
    {
        spawnCount++;

        for(int i = 0; i < enemyCount; i++)
        {
            GameObject tmpEnemy = Instantiate(enemyPrefab);
            tmpEnemy.transform.SetParent(gameObject.transform, false);

            Transform startCellPos = LM.wayPoints[0].transform;
            Vector3 startPos = new Vector3(startCellPos.position.x + startCellPos.GetComponent<SpriteRenderer>().bounds.size.x / 2,
                                           startCellPos.position.y + startCellPos.GetComponent<SpriteRenderer>().bounds.size.y);

            tmpEnemy.transform.position = startPos;

            yield return new WaitForSeconds(0.2f);
        }

    }

}
