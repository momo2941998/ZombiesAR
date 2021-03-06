﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerHp = 0;
    public bool isAlive;

    public void DecreaseHp(float _hp)
    {
        playerHp -= _hp;
        if (playerHp < 0) playerHp = 0;
    }

    public void IncreaseHp(float _hp)
    {
        playerHp += _hp;
    }

    public void InitHp(float hp)
    {
        playerHp = hp;
    }

}
