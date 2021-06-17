using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Train : MonoBehaviour, IPooledObject
{
    public ObjectPuller.ObjectInfo.ObjectType Type => type;

    [SerializeField]
    private ObjectPuller.ObjectInfo.ObjectType type;
    private const float trainWidth = 15;

    public void StartDriving(GameObject asphaltBlock)
    {
        transform.position =
            new Vector3(((SurfaceController.BlockWidth + trainWidth) / 2) *
                asphaltBlock.GetComponent<AsphaltBlock>().DirSign,
            1,
            asphaltBlock.transform.position.z);
        transform.
            DOMoveX(-transform.position.x, 5 / asphaltBlock.GetComponent<AsphaltBlock>().transportSpeed).
            OnComplete(() => ObjectPuller.Instance.DestroyObject(gameObject));
    }
}
