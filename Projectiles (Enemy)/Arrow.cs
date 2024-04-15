using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Arrow : EnemyProjectile
{
    public override void OnHit(Spirit hit)
    {
        owner.OnProjectileHit(hit);
    }
}
