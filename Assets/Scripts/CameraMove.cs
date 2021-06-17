using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    private Vector3 fixedDifference;
    private Sequence seq;
    private int lastZ;
    private bool isBlowing;
    // Start is called before the first frame update
    void Start()
    {
        isBlowing = false;
        lastZ = Mathf.FloorToInt(transform.position.z);
        seq = DOTween.Sequence();
        fixedDifference = Player.transform.position - transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameController.Instance.gameStatus == GameController.GameStatus.MAIN) && !isBlowing)
        {
            if (Mathf.FloorToInt(transform.position.z) > lastZ)
            {
                for (int i = 0; i < Mathf.FloorToInt(transform.position.z) - lastZ; i++)
                    SurfaceController.Instance.UpdateSurface();
                lastZ = Mathf.FloorToInt(transform.position.z);
            }
            transform.DOMoveZ(transform.position.z + 0.03f, 0.1f);
            if (fixedDifference.z - (Player.transform.position.z - transform.position.z) > 3)
            {
                isBlowing = true;
                transform.DOKill();
                transform.DOMove(Player.transform.position - fixedDifference, 0.1f);
                Player.GetComponent<PlayerController>().PlayerBlow();
            }
            if (fixedDifference.z < Player.transform.position.z - transform.position.z)
            {
                Vector3 dir = fixedDifference - (Player.transform.position - transform.position);
                transform.DOMove(transform.position - dir, 1 / dir.magnitude);
            }
            else
            {
                if(fixedDifference.x != Player.transform.position.x - transform.position.x)
                {
                    float xOffset = fixedDifference.x - (Player.transform.position.x - transform.position.x);
                    transform.DOMoveX(transform.position.x - xOffset, 1 / Mathf.Abs(xOffset));
                }
            }
        }
        else
        {
            if(!isBlowing)
                transform.DOKill();
        }
    }
}
