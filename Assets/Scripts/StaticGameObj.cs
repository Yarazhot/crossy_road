using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticGameObj : MonoBehaviour, IPooledObject
{
    public ObjectPuller.ObjectInfo.ObjectType Type => type;

    [SerializeField]
    private ObjectPuller.ObjectInfo.ObjectType type;
    public GameObject parentBlock;

    public void Init(GameObject parentBlock)
    {
        this.parentBlock = parentBlock;
        transform.position = parentBlock.transform.position;
    }

    public void SetRandomRotation()
    {
        transform.Rotate(new Vector3(0, 90 * Random.Range(0, 4)));
    }

    public void SetYOffset(float yOffset)
    {
        transform.position = transform.position + Vector3.up * yOffset;
    }

    public void SetZOffset(float zOffset)
    {
        transform.position = transform.position + Vector3.forward * zOffset;
    }

    public void SetXOffset(float xOffset)
    {
        transform.position = transform.position + Vector3.right * xOffset;
    }

    protected virtual void CheckParent()
    {
        if ((!parentBlock?.activeSelf??true) ||
            (parentBlock?.transform.position.z != gameObject.transform.position.z))
            ObjectPuller.Instance.DestroyObject(gameObject);
    }

    void Update()
    {
        CheckParent();
    }
}
