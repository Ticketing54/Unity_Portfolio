using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMark : MonoBehaviour
{
    public QUESTMARKTYPE MarkType { get; set; }   
    

      
    private void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime*20, 0));
    }
}
