using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    List<GameObject> wayPoints = new List<GameObject>();

    int wayIndex = 0;
    public int speed = 10;
    int health = 30;

    private void Start()
    {
        GetWaypoints();
    }

    void Update()
    {
        Move();
        CheckIsAlive();
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

        transform.Translate(dir.normalized * Time.deltaTime * speed);

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

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void CheckIsAlive()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
