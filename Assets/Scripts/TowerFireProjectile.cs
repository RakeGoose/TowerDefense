using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFireProjectile : MonoBehaviour
{

    Transform target;
    TowerProjectile selfProjectile;
    public Tower selfTower;
    gameController gcontroller;

    private void Start()
    {
        gcontroller = FindObjectOfType<gameController>();

        selfProjectile = gcontroller.AllProjectiles[selfTower.type];

        GetComponent<SpriteRenderer>().sprite = selfProjectile.Spr;
    }

    void Update()
    {
        Move();
    }

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    private void Move()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) < .1f)
            {
                Hit();
            }
            else
            {
                Vector2 dir = target.position - transform.position;
                transform.Translate(dir.normalized * Time.deltaTime * selfProjectile.speed);
            }
        }
        else
            Destroy(gameObject);
    }

    void Hit()
    {
        switch(selfTower.type)
        {
            case (int)TowerType.FIRST_TOWER:
                target.GetComponent<EnemyLogic>().AOEDamage(2, selfProjectile.damage);
                break;
            case (int)TowerType.SECOND_TOWER:
                target.GetComponent<EnemyLogic>().StartSlow(3, 1);
                target.GetComponent<EnemyLogic>().TakeDamage(selfProjectile.damage);
                break;
        }
        Destroy(gameObject);
    }
}
