using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{    
    protected string    unitName;
    protected string    sound;
    protected float     nick_YPos;
    protected bool      isTarget    = false;
    protected bool      usingUi     = false;
    public string NickName { get=> unitName; }
    public float Nick_YPos { get => nick_YPos; }
    public bool IsTarget { get => isTarget; }    
    public float DISTANCE
    {
        get
        {
            if (Character.Player == null)
            {
                return 0;
            }
            else
            {
                return Vector3.Distance(Character.Player.transform.position, this.gameObject.transform.position);
            }
        }
    }
    public Vector3 DIRECTION
    {
        get
        {
            if (Character.Player == null)
            {
                return Vector3.zero;
            }
            else
            {
                return Character.Player.transform.position - this.transform.position;
            }
        }
    }
   
}

