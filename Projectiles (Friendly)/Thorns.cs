using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Thorns : FriendlyProjectile
{
    public Animator anim;

    public override void OnHit(Tower hit)
    {
        StartCoroutine(DestroyWhenReady(hit));
    }

    IEnumerator DestroyWhenReady(Tower hit)
    {
        yield return new WaitForSeconds(0.2f);
        owner.OnProjectileHit(hit);
        while(!anim.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
