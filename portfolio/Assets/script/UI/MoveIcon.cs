using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveIcon : MonoBehaviour
{
    [SerializeField]
    Image iconImage;

    public void ActiveMoveIcon(Sprite _sprite)
    {
        iconImage.sprite = _sprite;
        StartCoroutine(StopMoveicon());
    }

    IEnumerator StopMoveicon()
    {
        while (true)
        {         
            if (Input.GetMouseButtonUp(0))
            {
                this.gameObject.SetActive(false);
                yield break;
            }
            gameObject.transform.position = Input.mousePosition;
            yield return null;
        }        
    }
  
    
}
