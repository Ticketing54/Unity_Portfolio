using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Coroutine uiUpdate;

    protected string    unitName;
    protected string    sound;
    protected float     nick_YPos;
    protected bool      isTarget    = false;
    protected bool      usingUi     = false;
    protected bool      usingDialog = false;
    public string NickName { get=> unitName; }
    public float Nick_YPos { get => nick_YPos; }
    public bool IsTarget { get => isTarget; }
    public bool UsingDialog { get => usingDialog; set => usingDialog = value; }

    public Vector3 startPos { get; set; }
    public List<string> wayPoint { get; set; }
    
    public virtual void OnEnable()
    {
        if(uiUpdate != null)
        {
            StopCoroutine(uiUpdate);            
        }

        GameManager.gameManager.character.removeNearUnit(this);
        uiUpdate =StartCoroutine(CoApproachChracter());
    }
    public virtual void OnDisable()
    {
        GameManager.gameManager.character.removeNearUnit(this);
        StopCoroutine(CoApproachChracter());
    }

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
    public Vector3 DIRECTION
    {
        get
        {
            if (GameManager.gameManager.character == null)
            {
                return Vector3.zero;
            }
            else
            {
                return GameManager.gameManager.character.transform.position - this.transform.position;
            }
        }
    }
    protected IEnumerator CoApproachChracter()
    {
        yield return null;
        bool approachChracter = false;

        while (true)
        {
            if (GameManager.gameManager.character != null)
            {
                if (this.DISTANCE < 6f && approachChracter == false)
                {
                    approachChracter = true;
                    GameManager.gameManager.character.addNearUnit(this);
                    UIManager.uimanager.uicontrol_On(this);
                }

                if (this.DISTANCE >= 6f && approachChracter == true)
                {
                    approachChracter = false;
                    GameManager.gameManager.character.removeNearUnit(this);
                    UIManager.uimanager.uicontrol_Off(this);
                }
            }
            yield return null;
        }
    }


}

