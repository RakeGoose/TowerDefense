using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    List<GameObject> wayPoints = new List<GameObject>();

    int wayIndex = 0;
    public Enemy selfEnemy;

    private void Start()
    {
        GetWaypoints();

        GetComponent<SpriteRenderer>().sprite = selfEnemy.Spr;
    }

    void Update()
    {
        Move();
    }

    void GetWaypoints()
    {
        wayPoints = GameObject.Find("LevelGroup").GetComponent<LevelManager>().wayPoints;
    }

    private void Move()
    {
        Transform currWayPoint = wayPoints[wayIndex].transform;
        Vector3 currWayPos = new Vector3(currWayPoint.position.x + currWayPoint.GetComponent<SpriteRenderer>().bounds.size.x / 2, currWayPoint.position.y - currWayPoint.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        Vector3 dir = currWayPos - transform.position;

        transform.Translate(dir.normalized * Time.deltaTime * selfEnemy.Speed);

        if(Vector3.Distance(transform.position, currWayPos) < 0.1f)
        {
            if(wayIndex < wayPoints.Count - 1)
            {
                wayIndex++;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        selfEnemy.Health -= damage;
        CheckIsAlive();
    }

    void CheckIsAlive()
    {
        if(selfEnemy.Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void StartSlow(float duration, float slowValue)
    {
        StopCoroutine("GetSlow");
        selfEnemy.Speed = selfEnemy.StartSpeed;
        StartCoroutine(GetSlow(duration, slowValue));
    }

    IEnumerator GetSlow(float duration, float slowValue)
    {
        selfEnemy.Speed -= slowValue;
        yield return new WaitForSeconds(duration);
        selfEnemy.Speed = selfEnemy.StartSpeed;
    }

    public void AOEDamage(float range, float damage)
    {
        List<EnemyLogic> enemies = new List<EnemyLogic>();

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(Vector2.Distance(transform.position, go.transform.position) <= range)
            {
                enemies.Add(go.GetComponent<EnemyLogic>());
            }
        }

        foreach(EnemyLogic es in enemies)
        {
            es.TakeDamage(damage);
        }
    }
}
