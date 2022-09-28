using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, UnitUiInfo
{
    protected string    unitName;
    protected string    sound;
    protected float     nick_YPos;
    protected bool      isTarget    = false;
    protected bool      usingUi     = false;
    protected bool      usingDialog = false;
    
    public bool UsingDialog { get => usingDialog; set => usingDialog = value; }

    public Vector3 startPos { get; set; }
    public List<string> wayPoint { get; set; }
    
    public float DISTANCE
    {
        get
        {
            if (GameManager.gameManager.character == null)
            {
                return 0;
            }
            else
            {
                return Vector3.Distance(GameManager.gameManager.character.transform.position, this.gameObject.transform.position);
            }
        }
    }

    public abstract float HpMax { get; set; }
    public abstract float HpCur { get; set; }
    

    public abstract string MiniDotSpriteName();
    public abstract bool IsEnermy();
  
    public string UnitName() { return unitName; }


    public Vector3 CurPostion() { return this.gameObject.transform.position; }

    public float Nick_YPos()  { return nick_YPos; }
}

