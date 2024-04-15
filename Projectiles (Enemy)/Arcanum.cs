using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Arcanum : EnemyProjectile
{
    public LayerMask mask;

    public override void OnHit(Spirit hit)
    {
        Collider2D[] impact = Physics2D.OverlapCircleAll(transform.position, 1.5f, mask);
        foreach(Collider2D collider in impact)
        {
            if(collider.transform.parent.gameObject.tag != "Spirit"){ continue; }
            owner.OnProjectileHit(collider.transform.parent.gameObject.GetComponent<Spirit>());
        }
        rb.velocity = Vector2.zero;
        Destroy(gameObject);
    }
}
