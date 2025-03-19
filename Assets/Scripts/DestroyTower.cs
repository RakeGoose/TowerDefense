using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTower : MonoBehaviour
{
    public cellScript SelfCell;

    public void Confirm()
    {
        SelfCell.DestroyTower();
        Cancel();
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
