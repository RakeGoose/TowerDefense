using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManagerScript : MonoBehaviour
{
    public static PointManagerScript Instance;
    public Text MoneyTxt;
    public int points;

    void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        MoneyTxt.text = points.ToString();
    }
}
