using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinSpawner : MonoBehaviour
{
    private Sequence seq;
    public static CoinSpawner Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        seq = DOTween.Sequence();
    }

    private void SpawnCoin(GameObject surfBlock)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponent<IPooledObject>() != null)
            {
                ObjectPuller.ObjectInfo.ObjectType type =
                    hit.transform.gameObject.GetComponent<IPooledObject>().Type;
                switch (type)
                {
                    case ObjectPuller.ObjectInfo.ObjectType.GRASS:
                        {
                            var coin = ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.COIN);
                            coin.GetComponent<StaticGameObj>().Init(surfBlock);
                            coin.GetComponent<StaticGameObj>().SetXOffset(transform.position.x);
                            //Debug.Log(coin.transform.position);
                            break;
                        }
                    case ObjectPuller.ObjectInfo.ObjectType.ASPHALT:
                    case ObjectPuller.ObjectInfo.ObjectType.LILLY_PAD:
                        {
                            var coin = ObjectPuller.Instance.GetObject(ObjectPuller.ObjectInfo.ObjectType.COIN);
                            coin.GetComponent<StaticGameObj>().Init(surfBlock);
                            coin.GetComponent<StaticGameObj>().SetXOffset(transform.position.x);
                            coin.GetComponent<StaticGameObj>().SetYOffset(-0.2f);
                            break;
                        }
                }
            }
        }
    }

    public void SpawnCoins(GameObject surfBlock)
    {
        seq.Append(transform.DOMove(surfBlock.transform.position, 0));
        seq.Append(transform.DOMoveY(4,0));
        for(int i = -4; i < 5; i++)
        {
            if (Random.Range(1, 20) == 10)
                seq.Append(transform.DOMoveX(i, 0).OnComplete(() => SpawnCoin(surfBlock)));
        }
    }
}
