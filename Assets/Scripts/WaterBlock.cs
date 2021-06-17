using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlock : SurfaceBlock
{
    public ObjectPuller.ObjectInfo.ObjectType waterObjType;
    public float waterSpeed;
    public float DirSign;

    public void InitBlock()
    {
        waterObjType = GenerateWaterObjType();
        waterSpeed = CarsController.Instance.BaseCarSpeed +
            Random.Range(4, 7);
        DirSign = WaterController.Instance.curWaterDirSign;
        WaterController.Instance.curWaterDirSign *= -1;
    }

    ObjectPuller.ObjectInfo.ObjectType GenerateWaterObjType()
    {
        if (WaterController.Instance.lastWaterObjType == ObjectPuller.ObjectInfo.ObjectType.LILLY_PAD)
            return WaterController.Instance.lastWaterObjType = ObjectPuller.ObjectInfo.ObjectType.LOG;
        switch (Random.Range(0, 4))
        {
            case 0: return WaterController.Instance.lastWaterObjType = ObjectPuller.ObjectInfo.ObjectType.LILLY_PAD;
            default: return WaterController.Instance.lastWaterObjType = ObjectPuller.ObjectInfo.ObjectType.LOG;
        }
    }
}
