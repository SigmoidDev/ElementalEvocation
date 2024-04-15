using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AirSwipe : FriendlyProjectile
{
    public override void Update()
    {
        base.Update();
        if(rb.velocity.sqrMagnitude <= 1.5f){ Destroy(gameObject); }
    }

    public override void OnHit(Tower hit)
    {
        owner.OnProjectileHit(hit);
        Destroy(gameObject);
    }
}
