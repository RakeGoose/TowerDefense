using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    public GameObject Menu;
    public Text MoneyTxt;
    public Text LivesTxt;
    public int points, livesCount;
    public bool canSpawn = false;

    public AudioClip LoseSound;
    public AudioClip EnemyDieSound;

    void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        MoneyTxt.text = points.ToString();
        LivesTxt.text = "LIVES: " + livesCount.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToMenu();
        }
    }

    public void PlayBtn()
    {
        FindObjectOfType<LevelManager>().CreateLevel();
        FindObjectOfType<EnemySpawner>().spawnCount = 0;
        FindObjectOfType<EnemySpawner>().timeToSpawn = 4;
        Menu.SetActive(false);
        canSpawn = true;
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    void ToMenu()
    {
        PlayLoseSound();
        FindObjectOfType<EnemySpawner>().StopAllCoroutines();
        foreach (EnemyLogic es in FindObjectsOfType<EnemyLogic>())
        {
            es.StopAllCoroutines();
            Destroy(es.gameObject);
        }
        foreach (TowerFireScript ts in FindObjectsOfType<TowerFireScript>())
            Destroy(ts.gameObject);
        foreach (cellScript cs in FindObjectsOfType<cellScript>())
            Destroy(cs.gameObject);

        points = 30;
        livesCount = 20;

        Menu.SetActive(true);
        canSpawn = false;

        if(FindObjectsOfType<ShopLogic>().Length > 0)
            Destroy(FindObjectOfType<ShopLogic>().gameObject);
    }

    public void TakePlayerDamage()
    {
        if(livesCount > 1)
        {
            livesCount--;
        }
        else
        {
            ToMenu();
        }
    }

    public void PlayLoseSound()
    {
        GetComponent<AudioSource>().clip = LoseSound;
        GetComponent<AudioSource>().Play();
    }

    public void PlayEnemyDieSound()
    {
        GetComponent<AudioSource>().clip = EnemyDieSound;
        GetComponent<AudioSource>().Play();
    }
}
