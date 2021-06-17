using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsphaltBlock : SurfaceBlock
{
    public ObjectPuller.ObjectInfo.ObjectType transportType;
    public float transportSpeed;
    public float DirSign;

    public void InitBlock()
    {
        transportType = GenerateTransportType();
        transportSpeed = CarsController.Instance.BaseCarSpeed +
            Random.Range(3, 6);
        DirSign = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    ObjectPuller.ObjectInfo.ObjectType GenerateTransportType()
    {
        switch (Random.Range(0, 4))
        {
            case 0: return ObjectPuller.ObjectInfo.ObjectType.CAR_1;
            case 1: return ObjectPuller.ObjectInfo.ObjectType.CAR_2;
            case 3: return ObjectPuller.ObjectInfo.ObjectType.CAR_3;
            default: return ObjectPuller.ObjectInfo.ObjectType.TRAIN;
        }
    }
}
