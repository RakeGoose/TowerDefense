using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLogic : MonoBehaviour
{
    public GameObject ItemPref;
    public Transform ItemGrid;

    gameController gcontroller;

    public cellScript selfCell;

    void Start()
    {
        gcontroller = FindObjectOfType<gameController>();

        foreach(Tower tower in gcontroller.AllTowers)
        {
            GameObject tmpItem = Instantiate(ItemPref);
            tmpItem.transform.SetParent(ItemGrid, false);
            tmpItem.GetComponent<ShopItemScript>().SetStartData(tower, selfCell);
        }
    }

    public void CloseShop()
    {
        Destroy(gameObject);
    }

}
