using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cellScript : MonoBehaviour
{
    public bool isGround;
    public bool hasTower = false;

    public Color BaseColor;
    public Color SelectColor;
    public Color DestroyColor;

    public GameObject ShopPref;
    public GameObject TowerPref;

    private void OnMouseEnter()
    {
        if (!isGround && FindObjectsOfType<ShopLogic>().Length == 0)
            if (!hasTower)
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
        if(!isGround && FindObjectsOfType<ShopLogic>().Length == 0)
        {
            if (!hasTower)
            {
                GameObject shopObj = Instantiate(ShopPref);
                shopObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
                shopObj.GetComponent<ShopLogic>().selfCell = this;
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

        hasTower = true;
        FindObjectOfType<ShopLogic>().CloseShop();
    }
}
