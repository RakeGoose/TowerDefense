using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFireScript : MonoBehaviour
{
    public GameObject Projectile;
    Tower selfTower;
    public TowerType selfType;
    
    gameController gcontroller;

    private void Start()
    {
        gcontroller = FindObjectOfType<gameController>();

        selfTower = gcontroller.AllTowers[(int)selfType];
        GetComponent<SpriteRenderer>().sprite = selfTower.Spr;

        InvokeRepeating("SearchTarget", 0, .1f);
    }

    private void Update()
    {
        if(selfTower.currCooldown > 0)
            selfTower.currCooldown -= Time.deltaTime;
        
    }

    bool CanShoot()
    {
        if(selfTower.currCooldown <= 0)
        {
            return true;
        }
        return false;
    }

    void SearchTarget()
    {
        if (CanShoot())
        {
            Transform nearestEnemy = null;
            float nearestEnemyDistance = Mathf.Infinity;

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                float currDistance = Vector2.Distance(transform.position, enemy.transform.position);

                if (currDistance < nearestEnemyDistance &&
                   currDistance <= selfTower.range)
                {
                    nearestEnemy = enemy.transform;
                    nearestEnemyDistance = currDistance;
                }
            }

            if (nearestEnemy != null)
                Shoot(nearestEnemy);
        }
    }

    void Shoot(Transform enemy)
    {
        selfTower.currCooldown = selfTower.Cooldown;

        GameObject proj = Instantiate(Projectile);
        proj.GetComponent<TowerFireProjectile>().selfTower = selfTower;
        proj.transform.position = transform.position;
        proj.GetComponent<TowerFireProjectile>().SetTarget(enemy);
    }
    
}
