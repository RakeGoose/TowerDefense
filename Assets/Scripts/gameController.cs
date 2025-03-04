using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower
{
    public int type;
    public float range = 0;
    public float Cooldown = 0;
    public float currCooldown = 0;
    public Sprite Spr;

    public Tower(int type, float range, float cd, string path)
    {
        this.type = type;
        this.range = range;
        Cooldown = cd;
        Spr = Resources.Load<Sprite>(path);
    }
}

public class TowerProjectile
{
    public float speed;
    public int damage;
    public Sprite Spr;

    public TowerProjectile(float speed, int dmg, string path)
    {
        this.speed = speed;
        damage = dmg;
        Spr = Resources.Load<Sprite>(path);
    }
}

public class Enemy
{
    public float Health;
    public float Speed;
    public float StartSpeed;
    public Sprite Spr;

    public Enemy(float health, float speed, string path)
    {
        Health = health;
        StartSpeed = Speed = speed;

        Spr = Resources.Load<Sprite>(path);
    }

    public Enemy(Enemy other)
    {
        Health = other.Health;
        StartSpeed = Speed = other.StartSpeed;
        Spr = other.Spr;
    }
}

public enum TowerType
{
    FIRST_TOWER,
    SECOND_TOWER
}

public class gameController : MonoBehaviour
{
    public List<Tower> AllTowers = new List<Tower>();
    public List<TowerProjectile> AllProjectiles = new List<TowerProjectile>();
    public List<Enemy> AllEnemies = new List<Enemy>();

    private void Awake()
    {
        AllTowers.Add(new Tower(0, 2, .5f, "TowerSprites/FireTower"));
        AllTowers.Add(new Tower(1, 5, 1.5f, "TowerSprites/FreezeTower"));

        AllProjectiles.Add(new TowerProjectile(7, 10, "ProjectilesSprites/FireProjectile"));
        AllProjectiles.Add(new TowerProjectile(7, 15, "ProjectilesSprites/FreezeProjectile"));

        AllEnemies.Add(new Enemy(30, 3, "EnemySprites/Enemy1"));
        AllEnemies.Add(new Enemy(20, 5, "EnemySprites/Enemy2"));
    }
}
