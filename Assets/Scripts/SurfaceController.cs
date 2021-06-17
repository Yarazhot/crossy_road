using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceController : MonoBehaviour
{
    public static SurfaceController Instance;
    public static float BlockWidth { get { return 45; } }
    public Queue<GameObject> SurfaceBlocks = new Queue<GameObject>();

    private const int SURFACE_LENGTH = 40;
    private const int BACK_PLATFORM_SIZE = 10;
    private const int START_PLARFORM_SIZE = 4;

    private ObjectPuller.ObjectInfo.ObjectType currentType;
    private float currentTypeBlockCount;
    private Vector3 currentPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        currentTypeBlockCount = 0;
        currentPosition = Vector3.forward * -BACK_PLATFORM_SIZE;
        for (int i = 0; i < BACK_PLATFORM_SIZE; i++)
            AddSurfaceBlock(ObjectPuller.ObjectInfo.ObjectType.GRASS, true);
        for (int i = 0; i < START_PLARFORM_SIZE; i++)
            AddSurfaceBlock(ObjectPuller.ObjectInfo.ObjectType.GRASS, false);
        for ( int i = START_PLARFORM_SIZE + BACK_PLATFORM_SIZE; i < SURFACE_LENGTH; i++)
            AddRandomSurfaceBlock();
    }

    public void UpdateSurface()
    {
        var buf = SurfaceBlocks.Dequeue();
        ObjectPuller.Instance.DestroyObject(buf);
        //AddSurfaceBlock(ObjectPuller.ObjectInfo.ObjectType.WATER);
        AddRandomSurfaceBlock();
    }

    void AddSurfaceBlock(ObjectPuller.ObjectInfo.ObjectType type, bool handle)
    {
        GameObject obj;
        SurfaceBlocks.Enqueue(obj = ObjectPuller.Instance.GetObject(type));
        obj.transform.position =  currentPosition;
        switch (type)
        {
            case ObjectPuller.ObjectInfo.ObjectType.ASPHALT:
                {
                    obj.GetComponent<AsphaltBlock>().InitBlock();
                    if(handle)
                        CarsController.Instance.HandleAsphaltBlock(obj);
                    break;
                }
            case ObjectPuller.ObjectInfo.ObjectType.WATER:
                {
                    obj.GetComponent<WaterBlock>().InitBlock();
                    if(handle)
                        WaterController.Instance.HandleWaterBlock(obj);
                    break;
                }
            case ObjectPuller.ObjectInfo.ObjectType.GRASS:
                {
                    GrassController.Instance.HandleGrassBlock(obj, handle);
                    break;
                }
        }
        CoinSpawner.Instance.SpawnCoins(obj);
        currentPosition = currentPosition + Vector3.forward;
    }

    void AddRandomSurfaceBlock()
    {
        if (currentTypeBlockCount <= 0)
        {
            var lastType = currentType;
            while (lastType == currentType)
                currentType = GenerateSurfType();
            currentTypeBlockCount = Random.Range(1, 5);
        }
        GameObject obj;
        SurfaceBlocks.Enqueue(obj = ObjectPuller.Instance.GetObject(currentType));
        obj.transform.position = currentPosition;
        switch (currentType)
        {
            case ObjectPuller.ObjectInfo.ObjectType.ASPHALT:
                {
                    obj.GetComponent<AsphaltBlock>().InitBlock();
                    CarsController.Instance.HandleAsphaltBlock(obj);
                    break;
                }
            case ObjectPuller.ObjectInfo.ObjectType.WATER:
                {
                    obj.GetComponent<WaterBlock>().InitBlock();
                    WaterController.Instance.HandleWaterBlock(obj);
                    break;
                }
            case ObjectPuller.ObjectInfo.ObjectType.GRASS:
                {
                    GrassController.Instance.HandleGrassBlock(obj, true);
                    break;
                }
        }
        CoinSpawner.Instance.SpawnCoins(obj);
        currentTypeBlockCount--;
        currentPosition = currentPosition + Vector3.forward;
    }

    ObjectPuller.ObjectInfo.ObjectType GenerateSurfType()
    {
        switch (Random.Range(0, 7))
        {
            case 0:
            case 1:
            case 2: return ObjectPuller.ObjectInfo.ObjectType.GRASS;
            case 3:
            case 4:
            case 5:  return ObjectPuller.ObjectInfo.ObjectType.ASPHALT;
            default: return ObjectPuller.ObjectInfo.ObjectType.WATER;
        }
    }
}
