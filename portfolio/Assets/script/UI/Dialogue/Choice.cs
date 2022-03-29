using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Choice : MonoBehaviour
{   
    public TextMeshProUGUI Text;    
    public Image clickImage;
    public int moveToDialogNum;
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
        if (pos.x >= RC.x && pos.x <= RC.x + RC.width && pos.y >= RC.y - RC.height && pos.y <= RC.y)
        {

            return true;
        }
        return false;
    }

    public void ClickEffect()
    {
        clickImage.gameObject.SetActive(true);
    }

    public void Cant_Choice()
    {
        Text.alpha = 0.3f;
        Text.color = Color.black;
    }
   
    public void ResetChoice()
    {        
        Text.alpha = 1f;
        Text.color = Color.white;        
        clickImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(clickImage.gameObject.activeSelf== true)
            {
                clickImage.gameObject.SetActive(false);
            }

        }
    }

}
