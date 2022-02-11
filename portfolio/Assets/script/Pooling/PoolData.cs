using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData<Type> where Type : Object
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

    public void Add(Type _PoolingObject)
    {
        Pool.Enqueue(_PoolingObject);
        count++;
    }
    public Type GetData()
    {
        if (count <= 0)
        {
            return Object.Instantiate<Type>(Prefab);
        }
        count--;

        return Pool.Dequeue();
    }

    public int Count { get { return count; } }
}
