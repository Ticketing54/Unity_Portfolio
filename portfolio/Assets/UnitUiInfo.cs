using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UnitUiInfo
{
    public string MiniDotSpriteName();
    public bool IsEnermy();    
    public string UnitName();
    public float HpMax { get; set; }
    public float HpCur { get; set; }
    public float Nick_YPos();
    public Vector3 CurPostion();

}
