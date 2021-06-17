using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Log : MonoBehaviour, IPooledObject
{
    public ObjectPuller.ObjectInfo.ObjectType Type => type;

    [SerializeField]
    private ObjectPuller.ObjectInfo.ObjectType type;

    private float logLength;

    private void SetRandomLength()
    {
        transform.localScale = new Vector3(Random.Range(2,4), 1, 1);
    }

    public void StartSailing(GameObject waterBlock)
    {
        SetRandomLength();
        transform.position =
            new Vector3(SurfaceController.BlockWidth / 3 * waterBlock.GetComponent<WaterBlock>().DirSign,
            0,
            waterBlock.transform.position.z);
        transform.DOMoveX(-transform.position.x, 35 / waterBlock.GetComponent<WaterBlock>().waterSpeed).
            SetEase(Ease.Linear).
            OnComplete(() => ObjectPuller.Instance.DestroyObject(gameObject));
    }
}
