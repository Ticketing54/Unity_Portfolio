using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomal_Npc : Npc
{
    public override float HpCur { get => -1; set { } }
    public override float HpMax { get => -1; set { } }
    public override bool IsEnermy() { return false; }
    
    
}
