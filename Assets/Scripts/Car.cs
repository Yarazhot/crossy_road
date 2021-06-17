using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Car : MonoBehaviour, IPooledObject
{
    public ObjectPuller.ObjectInfo.ObjectType Type => type;

    [SerializeField]
    private ObjectPuller.ObjectInfo.ObjectType type;
    
    public void StartDriving (GameObject asphaltBlock)
    {
        if(asphaltBlock.GetComponent<AsphaltBlock>().DirSign < 0)
            transform.rotation = Quaternion.Euler(0,180,0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position =
            new Vector3(SurfaceController.BlockWidth / 3 * asphaltBlock.GetComponent<AsphaltBlock>().DirSign,
            0.3f,
            asphaltBlock.transform.position.z);
        transform.
            DOMoveX(-transform.position.x, 25 / asphaltBlock.GetComponent<AsphaltBlock>().transportSpeed).
            SetEase(Ease.Linear).
            OnComplete(() => ObjectPuller.Instance.DestroyObject(gameObject));
    }
}
