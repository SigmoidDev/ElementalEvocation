using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Cannonball : EnemyProjectile
{
    public override void OnHit(Spirit hit)
    {
        owner.OnProjectileHit(hit);
        Destroy(gameObject);
    }
}
