using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMark : MonoBehaviour
{
    public QUESTMARKTYPE MarkType { get; set; }   
    Material material;

    private void Awake()
    {
        //material = GetComponent<Material>();
    }    
    public void StartingOrComplete()
    {
        //material.color = Color.yellow;            
    }    
    public void Playing()
    {
        //material.color = Color.gray;
    }    
    private void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime*20, 0));
    }
}
