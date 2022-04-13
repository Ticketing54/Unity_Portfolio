using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcUnit : Unit
{
    [SerializeField]
    protected List<int> quests;    
    [SerializeField]
    protected List<int> items;
    [SerializeField]
    protected string dialogue;
    protected int npcIndex;
    public List<int> QUEST { get => quests; }
    public List<int> ITEMS { get => items; }    
    public string DIALOGUE { get => dialogue; }


    
}

