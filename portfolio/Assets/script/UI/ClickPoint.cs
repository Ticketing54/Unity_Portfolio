﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPoint : MonoBehaviour
{
    float Dis;
    public float DiSTANCE
    {
        get
        {
            Dis = Vector3.Distance(Character.Player.transform.position, this.transform.position);
            return Dis;
        }
    }    
    
    void Update()
    {
       
    }
}
