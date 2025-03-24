using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    List<GameObject> wayPoints = new List<GameObject>();

    int wayIndex = 0;
    public Enemy selfEnemy;
    Animator animator;
    Vector2 lastPosition;
    bool isDead = false;

    private void Start()
    {
        GetWaypoints();

        GetComponent<SpriteRenderer>().sprite = selfEnemy.Spr;

        animator = GetComponent<Animator>();
        if(animator != null && selfEnemy.AnimController != null)
        {
            animator.runtimeAnimatorController = selfEnemy.AnimController;
            animator.SetInteger("Direction", 3);
        }
        Debug.Log("AnimatorController = " + selfEnemy.AnimController);


        lastPosition = transform.position;
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
        if (isDead)
        {
            return;
        }

        Transform currWayPoint = wayPoints[wayIndex].transform;
        Vector3 currWayPos = new Vector3(currWayPoint.position.x + currWayPoint.GetComponent<SpriteRenderer>().bounds.size.x / 2, currWayPoint.position.y - currWayPoint.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        Vector3 dir = currWayPos - transform.position;

        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            transform.localScale = new Vector3(dir.x > 0 ? -1 : 1, 1, 1);
        }

        if(animator != null)
        {
            Vector2 moveDir = dir.normalized;
            if(Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
            {
                animator.SetInteger("Direction", moveDir.x > 0 ? 1 : 0);
            }
            else
            {
                animator.SetInteger("Direction", moveDir.y > 0 ? 2 : 3);
            }
        }

        transform.Translate(dir.normalized * Time.deltaTime * selfEnemy.Speed);

        if(Vector3.Distance(transform.position, currWayPos) < 0.1f)
        {
            if(wayIndex < wayPoints.Count - 1)
            {
                wayIndex++;
            }
            else
            {
                GameManagerScript.Instance.TakePlayerDamage();
                Destroy(gameObject);
            }
        }
        lastPosition = transform.position;
    }

    public void TakeDamage(float damage)
    {
        selfEnemy.Health -= damage;
        CheckIsAlive();
    }

    void CheckIsAlive()
    {
        if(selfEnemy.Health <= 0 && !isDead)
        {
            isDead = true;
            GameManagerScript.Instance.points += 10;
            GameManagerScript.Instance.PlayEnemyDieSound();

            if(animator != null)
            {
                animator.SetBool("IsDead", true);
                StartCoroutine(WaitAndDestroy(animator.GetCurrentAnimatorStateInfo(0).length));
            }
            else
            {
                Destroy(gameObject);
            }
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

    IEnumerator WaitAndDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
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

    private void OnDestroy()
    {
        if (GameManagerScript.Instance != null && GameManagerScript.Instance.canSpawn)
        {
            EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
            if (spawner != null)
            {
                spawner.OnEnemyKilled();
            }
        }
    }
}
