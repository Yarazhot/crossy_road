using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : StaticGameObj
{
    protected override void CheckParent()
    {
        //nothing
    }

    public void DestroyAfterInterval(float interval)
    {
        StartCoroutine(DestroyCoroutine(interval));
    }

    IEnumerator DestroyCoroutine(float interval)
    {
        yield return new WaitForSeconds(interval);
        ObjectPuller.Instance.DestroyObject(gameObject);
    }

}
