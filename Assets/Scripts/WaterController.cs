using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public static WaterController Instance;
    public ObjectPuller.ObjectInfo.ObjectType lastWaterObjType = ObjectPuller.ObjectInfo.ObjectType.LOG;
    public float curWaterDirSign = 1;

    //List<GameObject> Cars = new List<GameObject>();
    private float baseWaterSpeed;
    public float BaseWaterSpeed { get { return baseWaterSpeed; } }

    public void IncreaseBaseWaterSpeed()
    {
        baseWaterSpeed++;
    }

    public void HandleWaterBlock(GameObject waterBlock)
    {
        if (waterBlock.GetComponent<WaterBlock>().waterObjType == ObjectPuller.ObjectInfo.ObjectType.LILLY_PAD)
            SpawnLillyPads(waterBlock);
        else
            StartSpawningLogs(waterBlock);
    }

    private void SpawnLillyPads(GameObject waterBlock)
    {
        float padsCount = Random.Range(1, 4);
        ArrayList reservedXOffset = new ArrayList();
        GameObject lillyPad = null;
        for (int i = 0; i < padsCount; i++)
        {
            try
            {
                lillyPad = ObjectPuller.Instance.GetObject(waterBlock.GetComponent<WaterBlock>().waterObjType);
                lillyPad.GetComponent<StaticGameObj>().Init(waterBlock);
                int buffOffset;
                while (reservedXOffset.Contains(buffOffset = Random.Range(-4, 5))) { }
                lillyPad.GetComponent<StaticGameObj>().SetXOffset(buffOffset);
                reservedXOffset.Add(buffOffset);
            }
            catch
            {
                if (lillyPad)
                    ObjectPuller.Instance.DestroyObject(lillyPad);
                Debug.Log("Problems with spawning lillypad");
            }
        }
        reservedXOffset.Clear();

    }

    private void StartSpawningLogs(GameObject waterBlock)
    {
        StartCoroutine(SpawnLog(waterBlock));
    }

    IEnumerator SpawnLog(GameObject waterBlock)
    {
        yield return new WaitForSeconds(1);
        GameObject log = null;
        try
        {
            if (waterBlock.activeSelf)
            {
                log = ObjectPuller.Instance.GetObject(waterBlock.GetComponent<WaterBlock>().waterObjType);
                log.GetComponent<Log>().StartSailing(waterBlock);
                StartCoroutine(SpawnLog(waterBlock));
            }
        }
        catch
        {
            if (log)
                ObjectPuller.Instance.DestroyObject(log);
            Debug.Log("Problems with spawning log");
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
}
