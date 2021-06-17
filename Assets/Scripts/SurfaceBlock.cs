using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceBlock : MonoBehaviour, IPooledObject
{
    public ObjectPuller.ObjectInfo.ObjectType Type => type;
    
    [SerializeField]
    private ObjectPuller.ObjectInfo.ObjectType type;

}
