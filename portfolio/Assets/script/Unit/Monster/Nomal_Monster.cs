using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomal_Monster : Monster
{
    private void Start()
    {
        base.Start();
        Character.Player.AddNearMonster(this);
    }
}
