using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PoolData<Type>where Type : MonoBehaviour
{
    protected Type Prefab;
    protected Queue<Type> Pool;
    protected int count = 0;

    public PoolData(Type _Prefab)
    {
        Pool = new Queue<Type>();
        Prefab = _Prefab;
        count = 0;
    }

    public abstract void Add(Type _PoolingObject);
    public abstract Type GetData();
    
    public int Count { get { return count; } }
}
