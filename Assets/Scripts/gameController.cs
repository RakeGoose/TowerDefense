using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tower
{
    public string Name;
    public int type;
    public int Price;
    public float range;
    public float Cooldown;
    public float currCooldown;
    public Sprite Spr;

    public Tower( string Name, int type, float range, float cd, int Price, string path)
    {
        this.Name = Name;
        this.type = type;
        this.range = range;
        Cooldown = cd;
        this.Price = Price;
        Spr = Resources.Load<Sprite>(path);
        currCooldown = 0;
    }
}

public struct TowerProjectile
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

public struct Enemy
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
        AllTowers.Add(new Tower("FireTower", 0, 2, .5f, 10, "TowerSprites/FireTower"));
        AllTowers.Add(new Tower("FreezeTower", 1, 5, 1.5f, 20, "TowerSprites/FreezeTower"));

        AllProjectiles.Add(new TowerProjectile(7, 10, "ProjectilesSprites/FireProjectile"));
        AllProjectiles.Add(new TowerProjectile(7, 15, "ProjectilesSprites/FreezeProjectile"));

        AllEnemies.Add(new Enemy(30, 3, "EnemySprites/Enemy1"));
        AllEnemies.Add(new Enemy(20, 5, "EnemySprites/Enemy2"));
    }
}
