using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{   
    [SerializeField]
    protected Image icon;    
    public Sprite ICON { get { return icon.sprite; } set { icon.sprite = value; } }
    public RectTransform tr;
    Rect rc;
    public Rect RC
    {
        get
        {
            rc.x = tr.position.x - tr.rect.width * 0.5f;
            rc.y = tr.position.y + tr.rect.height * 0.5f;
            return rc;
        }
    }
    void Start()
    {
        rc.x = tr.transform.position.x - tr.rect.width / 2;
        rc.y = tr.transform.position.y + tr.rect.height / 2;
        rc.xMax = tr.rect.width;
        rc.yMax = tr.rect.height;
        rc.width = tr.rect.width;
        rc.height = tr.rect.height;       
    }    
    public bool isInRect(Vector2 pos)
    {
        if (pos.x >= RC.x &&pos.x <= RC.x + RC.width &&pos.y >= RC.y - RC.height &&pos.y <= RC.y)
        {            
            return true;
        }
        return false;
    }        
   
    public bool isEmpty()
    {
        return ICON == null;
    }
    public virtual void Clear()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
    }
    public virtual void Add(string _SpriteName)
    {
        if (_SpriteName == string.Empty)
        {
            Clear();
        }
        icon.gameObject.SetActive(true);
        icon.sprite = ResourceManager.resource.GetImage(_SpriteName);
    }   

}
