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

    private void OnMouseEnter()
    {
        if (!isGround)
        {
            GetComponent<SpriteRenderer>().color = SelectColor;
        }
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = BaseColor;
    }
}
