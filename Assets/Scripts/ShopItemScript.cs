using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Tower selfTower;
    cellScript selfCell;
    public Image TowerLogo;
    public TextMeshProUGUI TowerName;
    public TextMeshProUGUI TowerPrice;

    public Color CurrColor;
    public Color BaseColor;

    public void SetStartData(Tower tower, cellScript cell)
    {
        selfTower = tower;
        TowerLogo.sprite = tower.Spr;
        TowerName.text = tower.Name;
        TowerPrice.text = tower.Price.ToString();
        selfCell = cell;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = CurrColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = BaseColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManagerScript.Instance.points >= selfTower.Price)
        {
            selfCell.BuildTower(selfTower);
            GameManagerScript.Instance.points -= selfTower.Price;
        }
    }

}
