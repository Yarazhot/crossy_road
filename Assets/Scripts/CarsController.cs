using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsController : MonoBehaviour
{
    public static CarsController Instance;

    List<GameObject> Cars = new List<GameObject>();
    private float baseCarSpeed;
    public float BaseCarSpeed { get { return baseCarSpeed; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void IncreaseBaseCarSpeed()
    {
        baseCarSpeed++;
    }

    public void HandleAsphaltBlock(GameObject surfBlock)
    {
        if (surfBlock.GetComponent<AsphaltBlock>().transportType == ObjectPuller.ObjectInfo.ObjectType.TRAIN)
        {
            SpawnRailway(surfBlock);
            StartSpawningTrains(surfBlock);
        }
        else
            StartSpawningCars(surfBlock);
    }

    private void SpawnRailway(GameObject surfBlock)
    {
        var railway = ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.RAILWAY);
        railway.GetComponent<StaticGameObj>().Init(surfBlock);
    }

    private void StartSpawningTrains(GameObject surfBlock)
    {
        StartCoroutine(GetReadyForTrain(surfBlock));
    }

    IEnumerator GetReadyForTrain(GameObject surfBlock)
    {
        yield return new WaitForSeconds(Random.Range(4, 10));
        GameObject light = null;
        GameObject light1 = null;
        try
        {
            if (surfBlock.activeSelf)
            {
                light = ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.SIGNAL_LIGHT);
                light.GetComponent<Light>().Init(surfBlock);
                light.GetComponent<Light>().SetXOffset(1.5f);
                light.GetComponent<Light>().SetYOffset(2.55f);
                light.GetComponent<Light>().SetZOffset(-0.4f);
                light.GetComponent<Light>().DestroyAfterInterval(2f);

                light1 = ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.SIGNAL_LIGHT);
                light1.GetComponent<Light>().Init(surfBlock);
                light1.GetComponent<Light>().SetXOffset(-1.5f);
                light1.GetComponent<Light>().SetYOffset(2.55f);
                light1.GetComponent<Light>().SetZOffset(-0.4f);
                light1.GetComponent<Light>().DestroyAfterInterval(2f);
                StartCoroutine(SpawnTrain(surfBlock));
            }
        }
        catch
        {
            if (light)
                ObjectPuller.Instance.DestroyObject(light);
            if (light1)
                ObjectPuller.Instance.DestroyObject(light1);
            Debug.Log("problems with spawning light");
        }
    }

    IEnumerator SpawnTrain(GameObject surfBlock)
    {
        yield return new WaitForSeconds(1.5f);
        GameObject transport = null;
        try
        {
            if (surfBlock.activeSelf)
            {
                transport = ObjectPuller.Instance.GetObject(surfBlock.GetComponent<AsphaltBlock>().transportType);
                transport.GetComponent<Train>().StartDriving(surfBlock);
                StartCoroutine(GetReadyForTrain(surfBlock));
            }
        }
        catch
        {
            if(transport)
                ObjectPuller.Instance.DestroyObject(transport);
            Debug.Log("Problems with spawning train");
        }
    }

    private void StartSpawningCars(GameObject surfBlock)
    {
        StartCoroutine(SpawnCar(surfBlock));
    }

    IEnumerator SpawnCar(GameObject surfBlock)
    {
        yield return new WaitForSeconds(Random.Range(2, 5));
        GameObject transport = null;
        try
        {
            if (surfBlock.activeSelf)
            {
                transport = ObjectPuller.Instance.GetObject(surfBlock.GetComponent<AsphaltBlock>().transportType);
                transport.GetComponent<Car>().StartDriving(surfBlock);
                StartCoroutine(SpawnCar(surfBlock));
            }
        }
        catch 
        {
            if (transport)
                ObjectPuller.Instance.DestroyObject(transport);
            Debug.Log("Problems with spawning car");
        }
    }
}
