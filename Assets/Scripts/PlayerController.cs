using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private bool isJumping;
    private bool isDead;
    private float distance = 1;
    private Sequence playerSeq;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip waterJump;
    [SerializeField]
    private AudioClip jump;
    [SerializeField]
    private AudioClip crush;
    [SerializeField]
    private AudioClip bubbles;

    void Start()
    {
        playerSeq = DOTween.Sequence();
        animator = GetComponent<Animator>();
        isJumping = false;
        isDead = false;
    }

    void Die()
    {
        isDead = true;
        GameController.Instance.PlayerDeath();
    }

    public void PlayerBlow()
    {
        isDead = true;
        audioSource.clip = bubbles;
        audioSource.loop = true;
        audioSource.Play();
        transform.DOShakeScale(1, 10).
            OnComplete(() => 
            {
                GameController.Instance.PlayerDeath();
                audioSource.loop = false;
                transform.DOMoveY(-5, 0);
            });

    }

    void Jump(Vector3 dir)
    {
        isJumping = true;
        animator.SetTrigger("jump");
        audioSource.clip = jump;
        audioSource.Play();
        if (transform.parent)
            transform.parent = null;
        playerSeq.Append( transform.DOMoveY(1, 0).
            OnComplete( () => transform.DOMove(transform.position + dir, 0.2f).SetEase(Ease.Linear).
            OnComplete(() => JumpCallBack())));
    }

    void JumpCallBack()
    {
        GameObject obj;
        isJumping = false;
        obj = CheckCurrentSurfaceType();
        switch (obj?.GetComponent<IPooledObject>().Type)
        {
            case ObjectPuller.ObjectInfo.ObjectType.GRASS:
            case ObjectPuller.ObjectInfo.ObjectType.ASPHALT:
                {
                    var x = Mathf.Round(transform.position.x);
                    playerSeq.Append(transform.DOMoveX(x, 0.05f));
                    break;
                }
            case ObjectPuller.ObjectInfo.ObjectType.LILLY_PAD:
                {
                    var x = Mathf.Round(transform.position.x);
                    playerSeq.Append(transform.DOMoveX(x, 0));
                    transform.parent = obj.transform;
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(obj.transform.DOLocalMoveY(-0.3f, 0.1f));
                    sequence.Append(obj.transform.DOLocalMoveY(0f, 0.1f));
                    break;
                }
            case ObjectPuller.ObjectInfo.ObjectType.LOG:
                {
                    transform.parent = obj.transform;
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(obj.transform.DOLocalMoveY(-0.3f, 0.1f));
                    sequence.Append(obj.transform.DOLocalMoveY(0f, 0.1f));
                    break;
                }
            case ObjectPuller.ObjectInfo.ObjectType.WATER:
                {
                    playerSeq.Append(transform.DOMoveY(-3, 0.5f));
                    //audioSource.PlayOneShot(waterJump);
                    audioSource.clip = waterJump;
                    audioSource.Play();
                    Die();
                    break;
                }
            case ObjectPuller.ObjectInfo.ObjectType.COIN:
                {
                    var x = Mathf.Round(transform.position.x);
                    playerSeq.Append(transform.DOMoveX(x, 0.05f));
                    ObjectPuller.Instance.DestroyObject(obj);
                    GameController.Instance.CoinsInc();
                    break;
                }
        }
    }

    GameObject CheckCurrentSurfaceType()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        GameObject result = null;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponent<IPooledObject>() != null)
                result = hit.transform.gameObject;
        }
        return result;
    }

    bool CheckCanJump(Vector3 dir)
    {
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;
        bool result = true;
        result = !( Physics.Raycast(ray, out hit) &&
            (hit.distance < distance) && hit.transform.gameObject.GetComponent<StaticGameObj>() ) &&
            !((dir == Vector3.left) && (transform.position.x == -4)) &&
            !((dir == Vector3.right) && (transform.position.x == 4));
        return result;
    }

    bool LetalJump()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;
        bool result = false;
        if (result = Physics.SphereCast(ray, 0.3f, out hit) &&
            (hit.distance < distance) && hit.transform.gameObject.GetComponent<Car>())
        {
            transform.parent = hit.transform;
            playerSeq.Append(transform.DOScaleZ(0.1f, 0));
            playerSeq.Append(transform.DOMoveZ(hit.transform.position.z - 0.5f, 0));
        }
        if (result)
            Die();
        return result;
    }
    IEnumerator GetOffTheLog()
    {
        yield return new WaitForSeconds(2.5f);
        transform.parent = null;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!isDead)
        {
            if (other.gameObject.GetComponent<Car>())
            {
                transform.DOKill();
                playerSeq.Append(transform.DOMoveZ(other.transform.position.z, 0));
                playerSeq.Append(transform.DOScaleY(0.1f, 0));
                playerSeq.Append(transform.DOMoveY(0.35f, 0));
                audioSource.clip = crush;
                audioSource.Play();
                Die();
            }
            if (other.gameObject.GetComponent<Train>())
            {
                transform.parent = other.transform;
                audioSource.clip = crush;
                audioSource.Play();
                Die();
            }
        }
    }

    void Update()
    {
        if ((transform.position.x > 4) || (transform.position.x < -4))
            Die();
        if (isDead)
        {
            if (transform.parent)
            {
                StartCoroutine(GetOffTheLog());
            }
        }
        if (!isJumping && !isDead &&
            (GameController.Instance.gameStatus == GameController.GameStatus.MAIN))
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                animator.SetTrigger("prepare");
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                transform.DORotate(new Vector3(0, 0, 0), 0.05f);
                if (CheckCanJump(Vector3.forward) && !LetalJump())
                    Jump(Vector3.forward);
                else
                    animator.SetTrigger("stopPrepare");
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                transform.DORotate(new Vector3(0, -90, 0), 0.05f);
                if (CheckCanJump(Vector3.left))
                    Jump(Vector3.left);
                else
                    animator.SetTrigger("stopPrepare");
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                transform.DORotate(new Vector3(0, 90, 0), 0.05f);
                if (CheckCanJump(Vector3.right))
                    Jump(Vector3.right);
                else
                    animator.SetTrigger("stopPrepare");
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                transform.DORotate(new Vector3(0, 180, 0), 0.05f);
                if (CheckCanJump(Vector3.back))
                    Jump(Vector3.back);
                else
                    animator.SetTrigger("stopPrepare");
            }
        }
    }
}
