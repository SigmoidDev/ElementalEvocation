using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Fireball : FriendlyProjectile
{
    public Animator anim;
    public LayerMask mask;

    public override void OnHit(Tower hit)
    {
        Collider2D[] impact = Physics2D.OverlapCircleAll(transform.position, 2.5f, mask);
        foreach(Collider2D collider in impact)
        {
            if(collider.gameObject.tag != "Tower"){ continue; }
            owner.OnProjectileHit(collider.gameObject.GetComponent<Tower>());
        }
        rb.velocity = Vector2.zero;
        anim.Play("Explosion");
    }

    public void DestroyCallback()
    {
        Destroy(gameObject);
    }
}
