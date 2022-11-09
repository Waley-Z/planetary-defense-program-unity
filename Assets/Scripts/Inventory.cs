using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public int money = 100000;
    public float rocks = 100;

    //public bool useMoney(int amount)
    //{
    //    if (amount > money)
    //        return false;
    //    money -= amount;
    //    return true;
    //}

    public bool useRocks(float amount)
    {
        if (amount > rocks)
        {
            ToastManager.ToastErrorMsg("insufficient rocks!");
            return false;
        }
        rocks -= amount;
        return true;
    }
}
