using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{
    public static GrassController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    public void HandleGrassBlock(GameObject grassBlock, bool handle)
    {
        SpawnGrassObjs(grassBlock, handle);
    }

    private void SpawnGrassObjs(GameObject grassBlock, bool handle)
    {
        ArrayList aa = new ArrayList();
        float objCount = 0;
        float a = Random.Range(0, 100);

        bool jkdf = ((objCount = a % 4) != 0)
            || ((objCount = a % 3) != 0) || ((objCount = a % 2) != 0)
            || ((objCount = a % 1) != 0);
       // if (objCount < 0)
        //    objCount = 0;
        ArrayList reservedXOffset = new ArrayList();
        GameObject grassObj = null;
        try
        {
            if (handle)
                for (int i = 0; i < objCount; i++)
                {
                    grassObj = Random.Range(0, 2) == 0 ?
                        ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.TREE) :
                        ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.ROCK);
                    grassObj.GetComponent<StaticGameObj>().Init(grassBlock);
                    grassObj.GetComponent<StaticGameObj>().SetRandomRotation();
                    float buffOffset;
                    while (reservedXOffset.Contains(buffOffset = Random.Range(-4, 5))) { }
                    grassObj.GetComponent<StaticGameObj>().SetXOffset(buffOffset);
                    reservedXOffset.Add(buffOffset);
                }
            grassObj = ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.TREE);
            grassObj.GetComponent<StaticGameObj>().Init(grassBlock);
            grassObj.GetComponent<StaticGameObj>().SetRandomRotation();
            grassObj.GetComponent<StaticGameObj>().SetXOffset(-5);
            grassObj = ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.TREE);
            grassObj.GetComponent<StaticGameObj>().Init(grassBlock);
            grassObj.GetComponent<StaticGameObj>().SetRandomRotation();
            grassObj.GetComponent<StaticGameObj>().SetXOffset(5);
            reservedXOffset.Clear();
            for (int i = -System.Convert.ToInt32(SurfaceController.BlockWidth / 2); i < -5; i++)
            {
                grassObj = Random.Range(0, 4) == 0 ?
                    null :
                    ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.TREE);
                if (grassObj)
                {
                    grassObj.GetComponent<StaticGameObj>().Init(grassBlock);
                    grassObj.GetComponent<StaticGameObj>().SetRandomRotation();
                    grassObj.GetComponent<StaticGameObj>().SetXOffset(i);
                }
            }
            for (int i = 6; i < System.Convert.ToInt32(SurfaceController.BlockWidth / 2); i++)
            {
                grassObj = Random.Range(0, 4) == 0 ?
                    null :
                    ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.TREE);
                if (grassObj)
                {
                    grassObj.GetComponent<StaticGameObj>().Init(grassBlock);
                    grassObj.GetComponent<StaticGameObj>().SetRandomRotation();
                    grassObj.GetComponent<StaticGameObj>().SetXOffset(i);
                }
            }
        }
        catch
        {
            if (grassObj)
                ObjectPuller.Instance.DestroyObject(grassObj);
            Debug.Log("Problems with spawning rock or tree");
        }
    }
}
