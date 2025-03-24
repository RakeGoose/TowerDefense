using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cellScript : MonoBehaviour
{
    public bool isGround;

    public Color BaseColor;
    public Color SelectColor;
    public Color DestroyColor;

    public GameObject ShopPref;
    public GameObject TowerPref;
    public GameObject DestroyPref;

    public GameObject SelfTower;

    private void OnMouseEnter()
    {
        if (!isGround && FindObjectsOfType<ShopLogic>().Length == 0
            && FindObjectsOfType<DestroyTower>().Length == 0)
            if (!SelfTower)
                GetComponent<SpriteRenderer>().color = SelectColor;
            else
                GetComponent<SpriteRenderer>().color = DestroyColor;
        
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = BaseColor;
    }

    private void OnMouseDown()
    {
        if(!isGround && FindObjectsOfType<ShopLogic>().Length == 0
            && GameManagerScript.Instance.canSpawn
            && FindObjectsOfType<DestroyTower>().Length == 0)
        {
            if (!SelfTower)
            {
                GameObject shopObj = Instantiate(ShopPref);
                shopObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
                shopObj.GetComponent<ShopLogic>().selfCell = this;
            }
            else
            {
                GameObject towerDestroy = Instantiate(DestroyPref);
                towerDestroy.transform.SetParent(GameObject.Find("Canvas").transform, false);
                towerDestroy.GetComponent<DestroyTower>().SelfCell = this;
            }
        }
    }

    public void BuildTower(Tower tower)
    {
        GameObject tmpTower = Instantiate(TowerPref);
        tmpTower.transform.SetParent(transform, false);
        Vector2 towerPos = new Vector2(transform.position.x + tmpTower.GetComponent<SpriteRenderer>().bounds.size.x / 2,
                                       transform.position.y - tmpTower.GetComponent<SpriteRenderer>().bounds.size.y / 2);
        tmpTower.transform.position = towerPos;

        tmpTower.GetComponent<TowerFireScript>().selfType = (TowerType)tower.type;

        SelfTower = tmpTower;
        FindObjectOfType<ShopLogic>().CloseShop();
        GameManagerScript.Instance.PlayPlaceTowerSound();
    }

    public void DestroyTower()
    {
        GameManagerScript.Instance.points += (SelfTower.GetComponent<TowerFireScript>().selfTower.Price / 2);
        Destroy(SelfTower);
    }
}
