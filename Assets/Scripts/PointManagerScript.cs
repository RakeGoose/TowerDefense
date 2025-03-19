using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    public GameObject Menu;
    public Text MoneyTxt;
    public int points;
    public bool canSpawn = false;

    void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        MoneyTxt.text = points.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToMenu();
        }
    }

    public void PlayBtn()
    {
        foreach (cellScript cs in FindObjectsOfType<cellScript>())
            Destroy(cs.gameObject);
        foreach (TowerFireScript ts in FindObjectsOfType<TowerFireScript>())
            Destroy(ts.gameObject);
        foreach (EnemyLogic es in FindObjectsOfType<EnemyLogic>())
            Destroy(es.gameObject);

        points = 30;

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
        Menu.SetActive(true);
        canSpawn = false;

        if(FindObjectsOfType<ShopLogic>().Length > 0)
            Destroy(FindObjectOfType<ShopLogic>().gameObject);
    }
}
