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

        Animator anim = GetComponent<Animator>();
        if(anim != null && selfProjectile.AnimController != null)
        {
            anim.runtimeAnimatorController = selfProjectile.AnimController;
        }
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

            Vector2 dir = target.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (Vector2.Distance(transform.position, target.position) < .1f)
            {
                Hit();
            }
            else
            {
                transform.Translate(Vector3.right * Time.deltaTime * selfProjectile.speed);
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
