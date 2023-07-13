using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_parameter : MonoBehaviour
{
    protected bool isAlive = true;
    public bool EnemyAlive()
    {
        return isAlive;
    }

    public void ChangeLiveStatus()
    {
        isAlive = !isAlive;
    }
}
