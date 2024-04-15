using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WaterBolt : FriendlyProjectile
{
    bool isHoming;

    public override void Update()
    {
        if(isHoming)
        {
            transform.right = (owner.transform.position - transform.position).normalized;
            rb.velocity = transform.right * 10f;

            if(Vector2.Distance(owner.transform.position, transform.position) < 0.3f)
            {
                owner.OnProjectileHit(null);
                Destroy(gameObject);
            }
        }
    }

    public override void OnHit(Tower hit)
    {
        isHoming = true;
    }
}
