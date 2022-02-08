using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface PoolData_Class
{
    void Reset();
}

public class PoolData_Class<Type> : PoolData<Type> where Type : MonoBehaviour,PoolData_Class
{    
    public PoolData_Class(Type _Prefab) : base(_Prefab) { _Prefab.Reset(); }   

    public override void  Add(Type _PoolingObject)
    {
        _PoolingObject.Reset();
        Pool.Enqueue(_PoolingObject);
        count++;
    }
    public override Type GetData()
    {
        if (count <= 0)
        {
            Type NewType = GameObject.Instantiate<Type>(Prefab);
            return NewType;
        }
        count--;
        return Pool.Dequeue();
    }    
}
