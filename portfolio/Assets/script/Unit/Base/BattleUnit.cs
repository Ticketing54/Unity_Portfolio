using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : Unit
{
    protected float hp_Max;
    protected float hp_Cur;
    protected float atk;
    protected int lev;
    protected int gold;
    protected int exp;
    protected List<int[]> items = new List<int[]>();
    public float HP_CURENT { get => hp_Cur; }
    public float Hp_Max { get => hp_Max; }

    public bool MightyEnermy()
    {
        if (Character.Player == null)
            return false;

        if (Character.Player.stat.LEVEL < this.lev)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


